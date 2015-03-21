using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Aldebaran.Proxies;
using NaoCoopLib.Helpers;

namespace NaoCoopLib.Executers
{
    /// <summary>
    /// Enum containing the robot commands in the synchronization process
    /// </summary>
    public enum SynchronizationCommand
    {
        Ready,
        Start,
        Ok
    }

    /// <summary>
    /// Robot synchronization executer class
    /// </summary>
    public class RobotSynchronization : ExecuterBase
    {
        #region Members
        SpeechRecognition _speechRecognition;
        bool _synchronized = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ip">The robot ip</param>
        /// <param name="port">The robot port</param>
        public RobotSynchronization(string ip, int port)
            : this(new NaoConnectionHelper(ip, port))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection">The robot connection object</param>
        public RobotSynchronization(NaoConnectionHelper connection)
            : base(connection)
        {
            // initialize members
            _speechRecognition = new SpeechRecognition(connection.IP, connection.Port);
            _speechRecognition.WordRecognized += _speechRecognition_WordRecognized;
        }

        
        #endregion

        #region Public
        /// <summary>
        /// Starts robot synchronization
        /// </summary>
        /// <returns></returns>
        public bool SynchronizeRobot()
        {
            this._synchronized = false;
            this.Say(SynchronizationCommand.Ready.ToString());
            this._speechRecognition.StartListening(new List<string>() { "Ready", "Ok", "Start" });
            while (!this._synchronized)
            {
                Thread.Sleep(10);
            }
            this._speechRecognition.StopListening();

            // confirmation received, sleep to the first x5 seconds
            TimeSpan span = new TimeSpan(DateTime.Now.Ticks);
            var toSleep = ((60 - span.Seconds) % 5) * 1000 - span.Milliseconds;
            if (toSleep <= 0) toSleep = 5000;
            var totalSleep = 0;
            while (this._synchronized && totalSleep < toSleep)
            {
                Thread.Sleep(10);
                totalSleep += 10;
            }
            return this._synchronized;
        }

        /// <summary>
        /// Cancells robot synchronization
        /// </summary>
        public void CancelSynchronization()
        {
            this._speechRecognition.StopListening();
            this._synchronized = false;
        }

        /// <summary>
        /// Disposes the current executer
        /// </summary>
        public override void Dispose()
        {
            this._speechRecognition.StopListening();
            this._speechRecognition.Dispose();
        }
        #endregion

        #region Private
        /// <summary>
        /// Word recognized event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _speechRecognition_WordRecognized(object sender, SpeechRecognitionEventArgs e)
        {
            SynchronizationCommand command = SynchronizationCommand.Ok;
            if (Enum.TryParse<SynchronizationCommand>(e.Word, out command))
            {
                switch (command)
                {
                    case SynchronizationCommand.Ready: 
                        // sent by slave robot, means it is in lift position
                        // send the start command and wait for confirmation
                        this.Say(SynchronizationCommand.Start.ToString());
                        break;
                    case SynchronizationCommand.Start:
                        // sent by master robot
                        // send the confirmation and set the synchronized variable to true
                        this.Say(SynchronizationCommand.Ok.ToString());
                        // allow time for the command to be received
                        Thread.Sleep(1000);
                        goto case SynchronizationCommand.Ok;
                    case SynchronizationCommand.Ok:
                        this._synchronized = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Makes robot say the specified word
        /// </summary>
        /// <param name="word"></param>
        void Say(string word)
        {
            using (var tts = _connection.GetProxy<TextToSpeechProxy>())
            {
                tts.say(word);
            }
        }
        #endregion

    }
}
