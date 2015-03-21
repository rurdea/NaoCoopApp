using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopDataAccess.Managers
{
    public class StateTasksManager : RecordsManagerBase<NaoCoopObjects.Classes.StateTask>
    {
        #region Members
        private TasksManager _tasksManager;
        #endregion

        public StateTasksManager(string connectionString)
            : base(connectionString)
        {
            _tasksManager = new TasksManager(connectionString);
        }

        protected override System.Data.Linq.ITable Records
        {
            get 
            {
                return base.DataContext.GetTable<NaoCoopDataAccess.StateTask>(); 
            }
        }
    }
}
