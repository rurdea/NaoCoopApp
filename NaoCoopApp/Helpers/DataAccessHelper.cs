using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NaoCoopDataAccess.Interfaces;
using NaoCoopDataAccess.Managers;
using NaoCoopObjects.Classes;

namespace NaoCoopApp.Helpers
{
    public class DataAccessHelper
    {
        #region Members
        private static DataAccessHelper _instance;
        #endregion

        #region Properties
        public static DataAccessHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataAccessHelper();
                }
                return _instance;
            }
        }

        public RobotsManager RobotsManager
        {
            get;
            private set;
        }

        public RobotVersionsManager RobotVersionsManager
        {
            get;
            private set;
        }

        public OperationsManager OperationsManager
        {
            get;
            private set;
        }

        public OperationRobotsManager OperationRobotsManager
        {
            get;
            private set;
        }

        public OperationRobotStatesManager OperationRobotStatesManager
        {
            get;
            private set;
        }

        public StatesManager StatesManager
        {
            get;
            private set;
        }

        public StateTasksManager StateTasksManager
        {
            get;
            private set;
        }

        public TasksManager TasksManager
        {
            get;
            private set;
        }

        public SettingsManager SettingsManager
        {
            get;
            private set;
        }

        public RequirementsManager RequirementsManager
        {
            get;
            private set;
        }

        public OperationRequirementsManager OperationRequirementsManager
        {
            get;
            private set;
        }

        public ExecutionsManager ExecutionsManager
        {
            get;
            private set;
        }

        public ExecutionRobotsManager ExecutionRobotsManager
        {
            get;
            private set;
        }

        public UsersManager UsersManager
        {
            get;
            private set;
        }
        #endregion

        #region Constructor
        private DataAccessHelper()
        {
            // load connection string
            var connectionString = NaoCoopApp.Properties.Settings.Default.NaoCoopDbConnectionString;
            // initialize managers
            RobotsManager = new RobotsManager(connectionString);
            OperationsManager = new OperationsManager(connectionString);
            OperationRobotsManager = new OperationRobotsManager(connectionString);
            OperationRobotStatesManager = new OperationRobotStatesManager(connectionString);
            StatesManager = new StatesManager(connectionString);
            StateTasksManager = new StateTasksManager(connectionString);
            TasksManager = new TasksManager(connectionString);
            SettingsManager = new SettingsManager(connectionString);
            RobotVersionsManager = new RobotVersionsManager(connectionString);
            RequirementsManager = new RequirementsManager(connectionString);
            OperationRequirementsManager = new OperationRequirementsManager(connectionString);
            ExecutionsManager = new ExecutionsManager(connectionString);
            ExecutionRobotsManager = new ExecutionRobotsManager(connectionString);
            UsersManager = new UsersManager(connectionString);
        }
        #endregion

        #region Methods
        public IRecordManager<T> GetRecordsManager<T>() where T : NaoCoopObject
        {
            if (typeof(T) == typeof(OperationRobot))
            {
                return (IRecordManager<T>)OperationRobotsManager;
            }
            else if (typeof(T) == typeof(Operation))
            {
                return (IRecordManager<T>)OperationsManager;
            }
            else if (typeof(T) == typeof(Robot))
            {
                return (IRecordManager<T>)RobotsManager;
            }
            else if (typeof(T) == typeof(RobotVersion))
            {
                return (IRecordManager<T>)RobotVersionsManager;
            }
            else if (typeof(T) == typeof(Task))
            {
                return (IRecordManager<T>)TasksManager;
            }
            else if (typeof(T) == typeof(State))
            {
                return (IRecordManager<T>)StatesManager;
            }
            else if (typeof(T) == typeof(OperationRobot))
            {
                return (IRecordManager<T>)OperationRobotsManager;
            }
            else if (typeof(T) == typeof(OperationRobotState))
            {
                return (IRecordManager<T>)OperationRobotStatesManager;
            }
            else if (typeof(T) == typeof(StateTask))
            {
                return (IRecordManager<T>)StateTasksManager;
            }
            else if (typeof(T) == typeof(Setting))
            {
                return (IRecordManager<T>)SettingsManager;
            }
            else if (typeof(T) == typeof(Requirement))
            {
                return (IRecordManager<T>)RequirementsManager;
            }
            else if (typeof(T) == typeof(OperationRequirement))
            {
                return (IRecordManager<T>)OperationRequirementsManager;
            }
            else if (typeof(T)==typeof(Execution))
            {
                return (IRecordManager<T>)ExecutionsManager;
            }
            else if (typeof(T) == typeof(ExecutionRobot))
            {
                return (IRecordManager<T>)ExecutionRobotsManager;
            }
            else if (typeof(T) == typeof(UsersManager))
            {
                return (IRecordManager<T>)UsersManager;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
