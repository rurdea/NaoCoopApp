using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using NaoCoopDataAccess.Interfaces;

namespace NaoCoopDataAccess.Managers
{
    public abstract class ManagerBase : IDisposable
    {
        #region Members
        private NaoCoopDataClassesDataContext _dataContext;
        #endregion

        public ManagerBase(string connectionString)
        {
            // test connection string
            // TO DO: implement logging and custom errors
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open(); // throws exception if invalid
                conn.Close();
            }

            this.ConnectionString = connectionString;
        }

        #region Properties
        internal NaoCoopDataClassesDataContext DataContext
        {
            get
            {
                if (_dataContext == null)
                {
                    this.InitializeContext();
                }

                return this._dataContext;
            }
        }

        public string ConnectionString
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            this.DisposeCurrentContext();
        }

        protected void InitializeContext()
        {
            this._dataContext = new NaoCoopDataClassesDataContext(this.ConnectionString);
        }

        protected void DisposeCurrentContext()
        {
            if (this._dataContext != null)
            {
                this._dataContext.Connection.Close();
                this._dataContext.Dispose();
            }
        }

        protected void ResetContext()
        {
            this.DisposeCurrentContext();
            this.InitializeContext();
        }
        #endregion
    }
}
