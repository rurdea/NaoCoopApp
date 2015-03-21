using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopDataAccess.Managers
{
    public class OperationRobotStatesManager : RecordsManagerBase<NaoCoopObjects.Classes.OperationRobotState>
    {
        #region Members
        private StatesManager _statesManager;
        #endregion

        public OperationRobotStatesManager(string connectionString)
            : base(connectionString)
        {
            _statesManager = new StatesManager(connectionString);
        }

        protected override System.Data.Linq.ITable Records
        {
            get 
            {
                return base.DataContext.GetTable<NaoCoopDataAccess.OperationRobotState>(); 
            }
        }

        public override void SaveRecord(NaoCoopObjects.Classes.OperationRobotState record)
        {
            // save this record
            base.SaveRecord(record);
            // save the state record
            //_statesManager.SaveRecord((NaoCoopObjects.Classes.State)record.State);
        }
    }
}
