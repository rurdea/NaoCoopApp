using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopDataAccess.Managers
{
    public class ExecutionsManager: RecordsManagerBase<NaoCoopObjects.Classes.Execution>
    {
        #region Members
        private ExecutionRobotsManager _executionRobotsManager;
        #endregion

        public ExecutionsManager(string connectionString)
            : base(connectionString)
        {
            // initialize other managers
            _executionRobotsManager = new ExecutionRobotsManager(connectionString);
        }

        protected override System.Data.Linq.ITable Records
        {
            get
            {
                return base.DataContext.GetTable<NaoCoopDataAccess.Execution>();
            }
        }

        public override void SaveRecord(NaoCoopObjects.Classes.Execution record)
        {
            // save this record
            base.SaveRecord(record);
            // save execution robtos
            foreach (var robot in record.ExecutionRobots)
            {
                _executionRobotsManager.SaveRecord(robot);
            }
        }
    }
}
