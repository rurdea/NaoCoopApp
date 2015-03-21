using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaoCoopApp.Helpers
{
    public class Singleton<T> where T : class
    {
        private static T _instance;
        public T Instance
        {
            get
            {
                return _instance ?? (_instance = (T)Activator.CreateInstance(typeof(T)));
            }
        }
    }
}
