using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopDataAccess.Managers
{
    public class OperationRobotsManager : RecordsManagerBase<NaoCoopObjects.Classes.OperationRobot>
    {
        #region Members
        private OperationRobotStatesManager _operationRobotStatesManager;
        #endregion

        public OperationRobotsManager(string connectionString)
            : base(connectionString)
        {
            // initialize other managers
            _operationRobotStatesManager = new OperationRobotStatesManager(connectionString);
        }

        protected override System.Data.Linq.ITable Records
        {
            get 
            {
                return base.DataContext.GetTable<NaoCoopDataAccess.OperationRobot>(); 
            }
        }

        public override void SaveRecord(NaoCoopObjects.Classes.OperationRobot record)
        {
            base.SaveRecord(record);
            // save state records
            foreach (var stateRecord in record.OperationRobotStates)
            {
                _operationRobotStatesManager.SaveRecord((NaoCoopObjects.Classes.OperationRobotState)stateRecord, record);
            }
        }

        internal override void SaveRecord(NaoCoopObjects.Classes.OperationRobot record, params object[] parentRecords)
        {
            base.SaveRecord(record, parentRecords);
            // save state records
            foreach (var stateRecord in record.OperationRobotStates)
            {
                _operationRobotStatesManager.SaveRecord((NaoCoopObjects.Classes.OperationRobotState)stateRecord, record);
            }
        }

        public override void DeleteRecord(NaoCoopObjects.Classes.OperationRobot record)
        {
            /*
            // delete the robot states first
            foreach (var robotState in record.RobotStates)
            {
                _robotStatesManager.DeleteRecord((NaoCoopObjects.Classes.RobotState)robotState);
            }*/
            // delete the robot
            base.DeleteRecord(record);
        }
    }
}
