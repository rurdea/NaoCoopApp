using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaoCoopApp.Validators;

namespace NaoCoopApp.Models
{
    public class Execution : ModelValidatorBase<NaoCoopObjects.Classes.Execution>
    {
        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public Execution()
        {
        }

        /// <summary>
        /// Constructor taking a nao execution object as parameter
        /// </summary>
        /// <param name="execution"></param>
        public Execution(NaoCoopObjects.Classes.Execution execution)
            : base(execution)
        {
            this.PendingDeleteExecutionRobots = new List<ExecutionRobot>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a list of robot in pending delete state
        /// </summary>
        public List<ExecutionRobot> PendingDeleteExecutionRobots
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime DateCreated
        {
            get
            {
                return Data.DateCreated;
            }
            set
            {
                if (Data.DateCreated != value)
                {
                    Data.DateCreated = value;
                    OnPropertyChanged(() => DateCreated);
                }
            }
        }

        public DateTime DateStarted
        {
            get
            {
                return Data.DateStarted;
            }
            set
            {
                if (Data.DateStarted != value)
                {
                    Data.DateStarted = value;
                    OnPropertyChanged(() => DateStarted);
                }
            }
        }

        public DateTime DateEnded
        {
            get
            {
                return Data.DateEnded;
            }
            set
            {
                if (Data.DateEnded != value)
                {
                    Data.DateCreated = value;
                    OnPropertyChanged(() => DateEnded);
                }
            }
        }

        private Operation _operation;
        public Operation Operation
        {
            get
            {
                if (_operation == null)
                {
                    _operation = new Operation(Data.Operation);
                }
                return _operation;
            }
            set
            {
                if (_operation != value)
                {
                    _operation = value;
                    Data.Operation = value.Data;
                    OnPropertyChanged(() => Operation);
                }
            }
        }

        public NaoCoopObjects.Classes.ExecutionStatus Status
        {
            get
            {
                return Data.Status;
            }
            set
            {
                if (Data.Status != value)
                {
                    Data.Status = value;
                    OnPropertyChanged(() => Status);
                }
            }
        }

        private ObservableCollection<ExecutionRobot> _executionRobots;
        /// <summary>
        /// Gets the robot associated with this operation
        /// </summary>
        public ObservableCollection<ExecutionRobot> ExecutionRobots
        {
            get
            {
                if (_executionRobots == null)
                {
                    _executionRobots = new ObservableCollection<ExecutionRobot>();
                    foreach (var executionRobot in Data.ExecutionRobots)
                    {
                        _executionRobots.Add(new ExecutionRobot(executionRobot as NaoCoopObjects.Classes.ExecutionRobot));
                    }
                    _executionRobots.CollectionChanged += _executionRobot_CollectionChanged;
                }

                return _executionRobots;
            }
            set
            {
                if (_executionRobots != value)
                {
                    if (_executionRobots != null)
                    {
                        _executionRobots.CollectionChanged -= _executionRobot_CollectionChanged;
                    }
                    _executionRobots = value;
                    if (_executionRobots != null)
                    {
                        _executionRobots.CollectionChanged += _executionRobot_CollectionChanged;
                    }
                    OnPropertyChanged(() => ExecutionRobots);
                }
            }
        }
        #endregion

        #region Handlers
        /// <summary>
        /// Robot collection changed event.
        /// Add old items to a pending delete state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _executionRobot_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach(var newItem in e.NewItems)
                {
                    Data.ExecutionRobots.Add(((ExecutionRobot)newItem).Data);
                }
            }
            if (e.OldItems != null)
            {
                foreach (var oldItem in e.OldItems)
                {
                    PendingDeleteExecutionRobots.Add(oldItem as ExecutionRobot);
                }
            }
        }
        #endregion
    }
}
