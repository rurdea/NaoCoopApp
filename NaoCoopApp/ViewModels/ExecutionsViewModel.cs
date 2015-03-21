using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using NaoCoopApp.Helpers;
using NaoCoopApp.Models;

namespace NaoCoopApp.ViewModels
{
    public class ExecutionsViewModel : RecordsViewModel<Execution, NaoCoopObjects.Classes.Execution>
    {
        #region Collections
        private ObservableCollection<RequirementInput> _availableRequirements;
        public ObservableCollection<RequirementInput> AvailableRequirements
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

        private ObservableCollection<Operation> _availableOperations;
        public ObservableCollection<Operation> AvailableOperations
        {
            get
            {
                return _availableOperations;
            }
            set
            {
                if (_availableOperations != value)
                {
                    _availableOperations = value;
                    OnPropertyChanged(() => AvailableOperations);
                }
            }
        }

        private ObservableCollection<Operation> _matchingOperations;
        public ObservableCollection<Operation> MatchingOperations
        {
            get
            {
                return _matchingOperations;
            }
            set
            {
                if (_matchingOperations != value)
                {
                    _matchingOperations = value;
                    OnPropertyChanged(() => MatchingOperations);
                }
            }
        }
        #endregion

        #region Commands
        public ICommand FindOperationCommand { get { return new DelegateCommand(OnFindOperation); } }
        public ICommand ExecuteOperationCommand { get { return new DelegateCommand(p => OnExecuteOperation((Operation)p)); } }
        #endregion

        public ExecutionsViewModel()
        {
            MatchingOperations = new ObservableCollection<Operation>();
            OnRefreshData();
        }

        #region Handlers
        protected override void OnRefreshData()
        {
            base.OnRefreshData();
            // refresh requirements
            var requirements = Helpers.DataAccessHelper.Instance.RequirementsManager.GetRecords();
            AvailableRequirements = base.InitializeRecordsCollection<RequirementInput, NaoCoopObjects.Classes.Requirement>(AvailableRequirements, requirements);
            // refresh operations
            var operations = Helpers.DataAccessHelper.Instance.OperationsManager.GetRecords();
            AvailableOperations = base.InitializeRecordsCollection<Operation, NaoCoopObjects.Classes.Operation>(AvailableOperations, operations);
        }

        protected override void OnSaveData()
        {
            // nothing to save for now
        }

        protected void OnFindOperation()
        {
            MatchingOperations.Clear();
            // collect all requirement values and find matching operation

            foreach (var operation in AvailableOperations)
            {
                bool isValid = true;
                foreach (var operationRequirement in operation.OperationRequirements)
                {
                    // get the requirement input
                    var requirementInput = AvailableRequirements.FirstOrDefault(r => r.ID.Equals(operationRequirement.Requirement.ID));
                    if (requirementInput == null || !ValidateOperationRequirement(operationRequirement, requirementInput.InputValue))
                    {
                        isValid = false;
                        break;
                    }
                }
                if (isValid)
                {
                    MatchingOperations.Add(operation);
                }
            }
        }

        protected void OnExecuteOperation(Operation operation)
        {
            if (operation != null)
            {
                NaoCoopApp.Views.RobotSelectionView robotSelectionView = new Views.RobotSelectionView();
                robotSelectionView.ShowDialog();
                if (robotSelectionView.DialogResult == DialogResult.Ok)
                {
                    // collect the robots to be used in the operation

                    // start the operation
                    NaoCoopApp.Views.OperationExecutionView operationExecutionView = new Views.OperationExecutionView(operation, robotSelectionView.SelectedRobots.ToArray());

                    try
                    {
                        // validate the execution
                        operationExecutionView.ValidateExecution();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("The execution is not valid. " + ex.Message);
                        return;
                    }

                    operationExecutionView.ShowDialog();
                }
            }
        }
        #endregion

        #region Private
        private bool ValidateOperationRequirement(OperationRequirement requirement, string inputValue)
        {
            try
            {
                if (requirement.Requirement.Type == NaoCoopObjects.Classes.RequirementType.@bool.ToString())
                {
                    bool referenceValue = Convert.ToBoolean(requirement.Value);
                    bool actualValue = Convert.ToBoolean(inputValue);
                    if (requirement.ValueValidator == NaoCoopObjects.Classes.OperationValueValidator.Equal.ToString())
                    {
                        return referenceValue.Equals(actualValue);
                    }
                    else if (requirement.ValueValidator == NaoCoopObjects.Classes.OperationValueValidator.NotEqual.ToString())
                    {
                        return !referenceValue.Equals(actualValue);
                    }
                }
                else if (requirement.Requirement.Type == NaoCoopObjects.Classes.RequirementType.@double.ToString())
                {
                    double referenceValue = Convert.ToDouble(requirement.Value);
                    double actualValue = Convert.ToDouble(inputValue);
                    if (requirement.ValueValidator == NaoCoopObjects.Classes.OperationValueValidator.Equal.ToString())
                    {
                        return referenceValue.Equals(actualValue);
                    }
                    else if (requirement.ValueValidator == NaoCoopObjects.Classes.OperationValueValidator.NotEqual.ToString())
                    {
                        return !referenceValue.Equals(actualValue);
                    }
                    else if (requirement.ValueValidator == NaoCoopObjects.Classes.OperationValueValidator.LessThan.ToString())
                    {
                        return actualValue < referenceValue;
                    }
                    else if (requirement.ValueValidator == NaoCoopObjects.Classes.OperationValueValidator.GreaterThan.ToString())
                    {
                        return actualValue > referenceValue;
                    }
                }
            }
            catch
            {
                // could not validate the requirement
            }
            return false;
        }
        #endregion
    }
}
