using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using NaoCoopLib.Helpers;
using Aldebaran.Proxies;
using System.Threading;
using System.Collections;

namespace NaoCoopLib.Executers
{
    #region SpeechRecognitionEventArgs
    /// <summary>
    /// SpeechRecognition event args class that contains the recognized word
    /// </summary>
    public class SpeechRecognitionEventArgs : EventArgs
    {
        #region Properties
        /// <summary>
        /// Gets the word
        /// </summary>
        public string Word
        {
            get;
            private set;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="word">Detected word</param>
        public SpeechRecognitionEventArgs(string word)
        {
            this.Word = word;
        }
        #endregion
    }
    #endregion

    public class SpeechRecognition : IDisposable
    {
        #region Constants
        private const int DEFAULT_INTERVAL = 500;
        private const string DEFAULT_LANGUAGE = "English";
        private const bool DEFAULT_WORD_SPOTTING = false;
        private const string SUBSCRIBER_NAME = "SpeechRecognition_Subscriber";
        private const float DEFAULT_MIN_PRECISION = 0.3f;
        #endregion

        #region Members
        private NaoConnectionHelper _connection;
        private BackgroundWorker _worker;
        #endregion

        #region Events
        public event EventHandler<SpeechRecognitionEventArgs> WordRecognized;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the language
        /// </summary>
        public string Language
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the words that the nao is set to recognize
        /// </summary>
        public List<string> RecognizedWords
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the flag indicating whether word spotting is enabled
        /// </summary>
        public bool EnableWordSpotting
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the interval in milliseconds for which the nao should look for recognized words
        /// </summary>
        public int Interval
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the minimum precision of a recognized word
        /// </summary>
        public float Precision
        {
            get;
            private set;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ip">The robot ip</param>
        /// <param name="port">The robot port</param>
        /// <param name="words">The words to monitor</param>
        /// <param name="language">The words language</param>
        /// <param name="wordSpotting">Word spotting option (full phrase or individual words)</param>
        /// <param name="interval">The interval between two memory reads</param>
        /// <param name="precision">Words precision</param>
        public SpeechRecognition(string ip, int port, List<string> words = null, string language = DEFAULT_LANGUAGE, bool wordSpotting = DEFAULT_WORD_SPOTTING, int interval = DEFAULT_INTERVAL, float precision = DEFAULT_MIN_PRECISION)
        {
            this.RecognizedWords = words;

            // check connection
            this._connection = new NaoConnectionHelper(ip, port);
            if (!this._connection.TestConnection())
            {
                throw new Exception("Could not connect to the Robot!");
            }

            this.Language = language;
            this.Interval = interval;
            this.EnableWordSpotting = wordSpotting;
            this.Precision = precision;

            // initialize worker
            _worker = new BackgroundWorker();
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += new DoWorkEventHandler(_worker_DoWork);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Starts listening for the specified words
        /// </summary>
        /// <param name="words"></param>
        /// <param name="enableWordSpotting"></param>
        public void StartListening(List<string> words, bool enableWordSpotting = DEFAULT_WORD_SPOTTING)
        {
            this.RecognizedWords = words;
            _worker.RunWorkerAsync();
        }

        /// <summary>
        /// Stops robot listening
        /// </summary>
        public void StopListening()
        {
            _worker.CancelAsync();
        }

        /// <summary>
        /// Disposes current object
        /// </summary>
        public void Dispose()
        {
            if (_worker.IsBusy)
            {
                _worker.CancelAsync();
            }
            _worker.Dispose();

            try
            {
                // make sure we unsubribe to speech recognition proxy
                using (var speechRecognitionProxy = _connection.GetProxy<SpeechRecognitionProxy>())
                {
                    speechRecognitionProxy.unsubscribe(SUBSCRIBER_NAME);
                }
            }
            catch {}
            _connection = null;
        }
        #endregion

        #region Private Methods
        #region Event Handlers
        /// <summary>
        /// Background do work event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // initialize words
            using (var speechRecognitionProxy = _connection.GetProxy<SpeechRecognitionProxy>())
            {
                
                try
                {   speechRecognitionProxy.unsubscribe(SUBSCRIBER_NAME); }
                catch
                {
                }

                speechRecognitionProxy.setLanguage(this.Language);
                speechRecognitionProxy.setVocabulary(this.RecognizedWords, this.EnableWordSpotting);
                //speechRecognitionProxy.setWordListAsVocabulary(this.RecognizedWords);
                speechRecognitionProxy.subscribe(SUBSCRIBER_NAME, this.Interval, 0f);

                while (!_worker.CancellationPending)
                {
                    using (var memProxy = _connection.GetProxy<MemoryProxy>())
                    {
                        var lastWordRecognized = memProxy.getData("LastWordRecognized");
                        var wordRecognized = memProxy.getData("WordRecognized") as ArrayList;

                        if (wordRecognized != null && 
                            wordRecognized.Count > 0 && 
                            !string.IsNullOrEmpty(wordRecognized[0].ToString()) &&
                            (float)wordRecognized[1]>=this.Precision)
                        {
                            if (WordRecognized != null)
                            {
                                speechRecognitionProxy.unsubscribe(SUBSCRIBER_NAME);
                                // get the first recognized word as the robot waits for one command at a time
                                string word = wordRecognized[0].ToString();
                                WordRecognized(this, new SpeechRecognitionEventArgs(word));
                                speechRecognitionProxy.subscribe(SUBSCRIBER_NAME, this.Interval, 0f);
                            }
                        }
                    }

                    if (!_worker.CancellationPending)
                    {
                        Thread.Sleep(this.Interval);
                    }
                }
                
                try
                {
                    speechRecognitionProxy.unsubscribe(SUBSCRIBER_NAME);
                }
                catch
                { // sometimes the dispose method executes faster than the worker thread 
                  // which will call the unsubscribe method faster than the worker thread
                  // therefore this catch is necessary
                }
            }
        }
        #endregion
        #endregion
    }
}
