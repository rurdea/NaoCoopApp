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
    public class Operation: ModelValidatorBase<NaoCoopObjects.Classes.Operation>
    {
        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public Operation()
        {
        }

        /// <summary>
        /// Constructor taking a nao robot object as parameter
        /// </summary>
        /// <param name="operation"></param>
        public Operation(NaoCoopObjects.Classes.Operation operation)
            : base(operation)
        {
            this.PendingDeleteOperationRobots = new List<OperationRobot>();
            this.PendingDeleteOperationRequirements = new List<OperationRequirement>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a list of robot states in pending delete state
        /// </summary>
        public List<OperationRobot> PendingDeleteOperationRobots
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a list of robot states in pending delete state
        /// </summary>
        public List<OperationRequirement> PendingDeleteOperationRequirements
        {
            get;
            private set;
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

        private ObservableCollection<OperationRobot> _operationRobots;
        /// <summary>
        /// Gets the robot associated with this operation
        /// </summary>
        public ObservableCollection<OperationRobot> OperationRobots
        {
            get
            {
                if (_operationRobots == null)
                {
                    _operationRobots = new ObservableCollection<OperationRobot>();
                    foreach (var operationRobot in Data.OperationRobots)
                    {
                        _operationRobots.Add(new OperationRobot(operationRobot as NaoCoopObjects.Classes.OperationRobot));
                    }
                    _operationRobots.CollectionChanged += _operationRobot_CollectionChanged;
                }

                return _operationRobots;
            }
            set
            {
                if (_operationRobots != value)
                {
                    if (_operationRobots != null)
                    {
                        _operationRobots.CollectionChanged -= _operationRobot_CollectionChanged;
                    }
                    _operationRobots = value;
                    if (_operationRobots != null)
                    {
                        _operationRobots.CollectionChanged += _operationRobot_CollectionChanged;
                    }
                    OnPropertyChanged(() => OperationRobots);
                }
            }
        }


          private ObservableCollection<OperationRequirement> _operationRequirements;
        /// <summary>
        /// Gets the robot associated with this operation
        /// </summary>
        public ObservableCollection<OperationRequirement> OperationRequirements
        {
            get
            {
                if (_operationRequirements == null)
                {
                    _operationRequirements = new ObservableCollection<OperationRequirement>();
                    foreach (var operationRequirement in Data.OperationRequirements)
                    {
                        _operationRequirements.Add(new OperationRequirement(operationRequirement as NaoCoopObjects.Classes.OperationRequirement));
                    }
                    _operationRequirements.CollectionChanged += _operationRequirement_CollectionChanged;
                }

                return _operationRequirements;
            }
            set
            {
                if (_operationRequirements != value)
                {
                    if (_operationRequirements != null)
                    {
                        _operationRequirements.CollectionChanged -= _operationRequirement_CollectionChanged;
                    }
                    _operationRequirements = value;
                    if (_operationRequirements != null)
                    {
                        _operationRequirements.CollectionChanged += _operationRequirement_CollectionChanged;
                    }
                    OnPropertyChanged(() => OperationRequirements);
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
        void _operationRobot_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach(var newItem in e.NewItems)
                {
                    Data.OperationRobots.Add(((OperationRobot)newItem).Data);
                }
            }
            if (e.OldItems != null)
            {
                foreach (var oldItem in e.OldItems)
                {
                    PendingDeleteOperationRobots.Add(oldItem as OperationRobot);
                }
            }
        }

        /// <summary>
        /// operation requirement collection changed event.
        /// Add old items to a pending delete state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _operationRequirement_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems)
                {
                    Data.OperationRequirements.Add(((OperationRequirement)newItem).Data);
                }
            }
            if (e.OldItems != null)
            {
                foreach (var oldItem in e.OldItems)
                {
                    PendingDeleteOperationRequirements.Add(oldItem as OperationRequirement);
                }
            }
        }
        #endregion

 
    }
}
