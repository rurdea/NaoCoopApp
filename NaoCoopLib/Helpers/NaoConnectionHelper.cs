using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace NaoCoopLib.Helpers
{
    public class NaoConnectionHelper
    {
        #region Properties
        public string IP
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public NaoConnectionHelper()
        {
        }

        public NaoConnectionHelper(string ip, int port)
        {
            this.IP = ip;
            this.Port = port;
        }
        #endregion

        #region Public Methods
        public bool TestConnection(string textToSpeech = null)
        {
            using (var textToSpeechProxy = this.GetProxy<Aldebaran.Proxies.TextToSpeechProxy>())
            {
                if (textToSpeechProxy != null)
                {
                    if (!string.IsNullOrWhiteSpace(textToSpeech))
                    {
                        textToSpeechProxy.say(textToSpeech);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public T GetProxy<T>()
        {
            return GetProxy<T>(this.IP, this.Port);
        }

        public static T GetProxy<T>(string ip, int port)
        {
            T instance = default(T);
            try
            {
                instance = (T)Activator.CreateInstance(typeof(T), ip, port);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error creating proxy instance. " + ex.ToString());
            }

            return instance;
        }
        #endregion
    }
}
