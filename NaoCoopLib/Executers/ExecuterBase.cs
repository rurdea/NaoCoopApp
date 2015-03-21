using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NaoCoopLib.Helpers;

namespace NaoCoopLib.Executers
{
    public abstract class ExecuterBase : IDisposable
    {
        #region Members
        protected NaoConnectionHelper _connection = new NaoConnectionHelper();
        #endregion

        public ExecuterBase(string ip, int port)
            : this(new NaoConnectionHelper(ip, port))
        {
        }

        public ExecuterBase(NaoConnectionHelper connection)
        {
            /*
            if (connection==null || !connection.TestConnection(string.Empty))
            {
                throw new ArgumentException("Could not connect to the robot!");
            }
            */

            this._connection = connection;
        }

        public abstract void Dispose();
    }
}
