using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopDataAccess.Managers
{
    public class OperationsManager : RecordsManagerBase<NaoCoopObjects.Classes.Operation>
    {
        #region Members
        private OperationRobotsManager _operationRobotsManager;
        private OperationRequirementsManager _operationRequirementsManager;
        #endregion

        public OperationsManager(string connectionString)
            : base(connectionString)
        {
            _operationRobotsManager = new OperationRobotsManager(connectionString);
            _operationRequirementsManager = new OperationRequirementsManager(connectionString);
        }

        protected override System.Data.Linq.ITable Records
        {
            get { return base.DataContext.GetTable<NaoCoopDataAccess.Operation>(); }
        }

        public override void SaveRecord(NaoCoopObjects.Classes.Operation record)
        {
            // save this record
            base.SaveRecord(record);
            // save operation robots
            foreach (var operationRobot in record.OperationRobots)
            {
                _operationRobotsManager.SaveRecord(operationRobot, record);
            }
            // save operation requirements
            foreach (var operationRequirement in record.OperationRequirements)
            {
                _operationRequirementsManager.SaveRecord(operationRequirement, record);
            }
        }
    }
}
