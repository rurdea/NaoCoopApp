using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaoCoopApp.Validators;

namespace NaoCoopApp.Models
{
    public class State : ModelValidatorBase<NaoCoopObjects.Classes.State>
    {
        #region Constructor
        public State()
            : this(null)
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="robot"></param>
        public State(NaoCoopObjects.Classes.State state)
            : base(state)
        {
            this.PendingDeleteStateTasks = new List<StateTask>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a list of state tasks in pending delete state
        /// </summary>
        public List<StateTask> PendingDeleteStateTasks
        {
            get;
            private set;
        }

        private ObservableCollection<StateTask> _stateTasks;
        /// <summary>
        /// Gets the robot states associated with this robot
        /// </summary>
        public ObservableCollection<StateTask> StateTasks
        {
            get
            {
                if (_stateTasks == null)
                {
                    _stateTasks = new ObservableCollection<StateTask>();
                    foreach (var robotState in Data.StateTasks)
                    {
                        _stateTasks.Add(new StateTask(robotState as NaoCoopObjects.Classes.StateTask));
                    }
                    _stateTasks.CollectionChanged += _stateTasks_CollectionChanged;
                }

                return _stateTasks;
            }
            set
            {
                if (_stateTasks != value)
                {
                    if (_stateTasks != null)
                    {
                        _stateTasks.CollectionChanged -= _stateTasks_CollectionChanged;
                    }
                    _stateTasks = value;
                    if (_stateTasks != null)
                    {
                        _stateTasks.CollectionChanged += _stateTasks_CollectionChanged;
                    }
                    OnPropertyChanged(() => StateTasks);
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

        public string Type
        {
            get
            {
                return Data.Type;
            }
            set
            {
                if (Data.Type != value)
                {
                    Data.Type = value;
                    OnPropertyChanged(() => Type);
                }
            }
        }
        #endregion

        #region Handlers
        /// <summary>
        /// State tasks collection changed event.
        /// Add old items to a pending delete state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _stateTasks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems)
                {
                    Data.StateTasks.Add(((StateTask)newItem).Data);
                }
            }
            if (e.OldItems != null)
            {
                foreach (var oldItem in e.OldItems)
                {
                    PendingDeleteStateTasks.Add(oldItem as StateTask);
                }
            }
        }
        #endregion
    }
}
