using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaoCoopApp.Models;

namespace NaoCoopApp.ViewModels
{
    public class OperationsViewModel : RecordsViewModel<Operation, NaoCoopObjects.Classes.Operation>
    {
        #region Properties
        private ObservableCollection<State> _availableStates;
        public ObservableCollection<State> AvailableStates
        {
            get { return _availableStates; }
            set
            {
                if (_availableStates != value)
                {
                    _availableStates = value;
                    OnPropertyChanged(() => AvailableStates);
                }
            }
        }

        private ObservableCollection<RobotVersion> _availableRobotVersions;
        public ObservableCollection<RobotVersion> AvailableRobotVersions
        {
            get
            {
                return _availableRobotVersions;
            }
            set
            {
                if (_availableRobotVersions != value)
                {
                    _availableRobotVersions = value;
                    OnPropertyChanged(() => AvailableRobotVersions);
                }
            }
        }

        private ObservableCollection<Requirement> _availableRequirements;
        public ObservableCollection<Requirement> AvailableRequirements
        {
            get
            {
                return _availableRequirements;
            }
            set
            {
                if (_availableRequirements != value)
                {
                    _availableRequirements = value;
                    OnPropertyChanged(() => AvailableRequirements);
                }
            }
        }

        private ObservableCollection<string> _availableValueValidators;
        public ObservableCollection<string> AvailableValueValidators
        {
            get
            {
                return _availableValueValidators;
            }
            set
            {
                if (_availableValueValidators != value)
                {
                    _availableValueValidators = value;
                    OnPropertyChanged(() => AvailableValueValidators);
                }
            }
        }

        
        #endregion

        #region Constructor
        public OperationsViewModel()
        {
            _availableValueValidators = new ObservableCollection<string>(Enum.GetNames(typeof(NaoCoopObjects.Classes.OperationValueValidator)));
        }
        #endregion

        #region Handlers
        protected override void OnRefreshData()
        {
            base.OnRefreshData();
            // refresh states
            var states = Helpers.DataAccessHelper.Instance.StatesManager.GetRecords();
            AvailableStates = base.InitializeRecordsCollection<State, NaoCoopObjects.Classes.State>(AvailableStates, states);
            // get robot versions from database
            var versions = Helpers.DataAccessHelper.Instance.RobotVersionsManager.GetRecords();
            AvailableRobotVersions = base.InitializeRecordsCollection<RobotVersion, NaoCoopObjects.Classes.RobotVersion>(AvailableRobotVersions, versions);
            // refresh requirements
            var requirements = Helpers.DataAccessHelper.Instance.RequirementsManager.GetRecords();
            AvailableRequirements = base.InitializeRecordsCollection<Requirement, NaoCoopObjects.Classes.Requirement>(AvailableRequirements, requirements);
        }

        protected override void OnSaveData()
        {
            base.OnSaveData();

            // delete pending delete objects
            foreach (var operation in RecordsCollection)
            {
                foreach (var operationRobot in operation.PendingDeleteOperationRobots)
                {
                    foreach (var operationRobotState in operationRobot.PendingDeleteOperationRobotStates)
                    {
                        Helpers.DataAccessHelper.Instance.OperationRobotStatesManager.DeleteRecord(operationRobotState.Data);
                    }
                    operationRobot.PendingDeleteOperationRobotStates.Clear();

                    Helpers.DataAccessHelper.Instance.OperationRobotsManager.DeleteRecord(operationRobot.Data);
                }
                operation.PendingDeleteOperationRobots.Clear();
            }
        }
        #endregion
    }
}
