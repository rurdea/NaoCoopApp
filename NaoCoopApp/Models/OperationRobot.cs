using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaoCoopApp.Helpers;
using NaoCoopApp.Validators;

namespace NaoCoopApp.Models
{
    public class OperationRobot : ModelValidatorBase<NaoCoopObjects.Classes.OperationRobot>
    {
        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public OperationRobot() : this(null)
        {
        }

        /// <summary>
        /// Constructor taking a nao robot object as parameter
        /// </summary>
        /// <param name="robot"></param>
        public OperationRobot(NaoCoopObjects.Classes.OperationRobot robot)
            : base(robot)
        {
            this.PendingDeleteOperationRobotStates = new List<OperationRobotState>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a list of robot states in pending delete state
        /// </summary>
        public List<OperationRobotState> PendingDeleteOperationRobotStates
        {
            get;
            private set;
        }

        private ObservableCollection<OperationRobotState> _operationRobotStates;
        private NaoCoopObjects.Classes.OperationRobotState operationRobotState;
        /// <summary>
        /// Gets the robot states associated with this robot
        /// </summary>
        public ObservableCollection<OperationRobotState> OperationRobotStates
        {
            get
            {
                if (_operationRobotStates == null)
                {
                    _operationRobotStates = new ObservableCollection<OperationRobotState>();
                    foreach (var robotState in Data.OperationRobotStates)
                    {
                        _operationRobotStates.Add(new OperationRobotState(robotState as NaoCoopObjects.Classes.OperationRobotState));
                    }
                    _operationRobotStates.CollectionChanged += _robotStates_CollectionChanged;
                }

                return _operationRobotStates;
            }
            set
            {
                if (_operationRobotStates != value)
                {
                    if (_operationRobotStates != null)
                    {
                        _operationRobotStates.CollectionChanged -= _robotStates_CollectionChanged;
                    }
                    _operationRobotStates = value;
                    if (_operationRobotStates != null)
                    {
                        _operationRobotStates.CollectionChanged += _robotStates_CollectionChanged;
                    }
                    OnPropertyChanged(() => OperationRobotStates);
                }
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a name for the Robot!")]
        public string Name
        {
            get
            {
                return Data.Name;
            }
            set
            {
                if (Data.Name != value)
                {
                    Data.Name = value;
                    OnPropertyChanged(() => Name);
                }
            }
        }

        private RobotVersion _robotVersion;
        public RobotVersion RobotVersion
        {
            get
            {
                if (_robotVersion == null)
                {
                    _robotVersion = new RobotVersion(Data.RobotVersion);
                }
                return _robotVersion;
            }
            set
            {
                if (_robotVersion != value)
                {
                    _robotVersion = value;
                    Data.RobotVersion = value.Data;
                    OnPropertyChanged(() => RobotVersion);
                }
            }
        }
        #endregion

        #region Handlers
        /// <summary>
        /// Robot States collection changed event.
        /// Add old items to a pending delete state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _robotStates_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach(var newItem in e.NewItems)
                {
                    Data.OperationRobotStates.Add(((OperationRobotState)newItem).Data);
                }
            }
            if (e.OldItems != null)
            {
                foreach (var oldItem in e.OldItems)
                {
                    PendingDeleteOperationRobotStates.Add(oldItem as OperationRobotState);
                }
            }
        }
        #endregion
    }
}
