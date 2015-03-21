using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaoCoopApp.Helpers;
using NaoCoopApp.Validators;

namespace NaoCoopApp.Models
{
    public class Task : ModelValidatorBase<NaoCoopObjects.Classes.Task>
    {
        #region Constructors
        public Task()
            : this (null)
        {
        }

        public Task(NaoCoopObjects.Classes.Task task)
            : base(task)
        {
            PendingDeleteSettings = new List<Setting>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a list of state tasks in pending delete state
        /// </summary>
        public List<Setting> PendingDeleteSettings
        {
            get;
            private set;
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a name for the Task!")]
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

        private ObservableCollection<Setting> _taskSettings;
        /// <summary>
        /// Gets the robot states associated with this robot
        /// </summary>
        public ObservableCollection<Setting> Settings
        {
            get
            {
                if (_taskSettings == null)
                {
                    _taskSettings = new ObservableCollection<Setting>();
                    foreach (var robotState in Data.Settings)
                    {
                        _taskSettings.Add(new Setting(robotState as NaoCoopObjects.Classes.Setting));
                    }
                    _taskSettings.CollectionChanged += _taskSettings_CollectionChanged;
                }

                return _taskSettings;
            }
            set
            {
                if (_taskSettings != value)
                {
                    if (_taskSettings != null)
                    {
                        _taskSettings.CollectionChanged -= _taskSettings_CollectionChanged;
                    }
                    _taskSettings = value;
                    if (_taskSettings != null)
                    {
                        _taskSettings.CollectionChanged += _taskSettings_CollectionChanged;
                    }
                    OnPropertyChanged(() => Settings);
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
        void _taskSettings_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var newItem in e.NewItems)
                {
                    Data.Settings.Add(((Setting)newItem).Data);
                }
            }
            if (e.OldItems != null)
            {
                foreach (var oldItem in e.OldItems)
                {
                    PendingDeleteSettings.Add(oldItem as Setting);
                }
            }
        }
        #endregion
    }
}
