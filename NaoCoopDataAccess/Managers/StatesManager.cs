using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopDataAccess.Managers
{
    public class StatesManager : RecordsManagerBase<NaoCoopObjects.Classes.State>
    {
        #region Members
        private StateTasksManager _stateTasksManager;
        #endregion

        public StatesManager(string connectionString)
            : base(connectionString)
        {
            _stateTasksManager = new StateTasksManager(connectionString);
        }

        protected override System.Data.Linq.ITable Records
        {
            get 
            {
                return base.DataContext.GetTable<NaoCoopDataAccess.State>(); 
            }
        }

        public override void SaveRecord(NaoCoopObjects.Classes.State record)
        {
            // save this record
            base.SaveRecord(record);
            // save the state task records
            foreach (var stateTask in record.StateTasks)
            {
                _stateTasksManager.SaveRecord((NaoCoopObjects.Classes.StateTask)stateTask, record);
            }
        }
    }
}
