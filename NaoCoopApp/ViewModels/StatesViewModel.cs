using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using NaoCoopApp.Helpers;
using NaoCoopApp.Models;

namespace NaoCoopApp.ViewModels
{
    public class StatesViewModel : RecordsViewModel<State, NaoCoopObjects.Classes.State>
    {
        #region Properties
        private ObservableCollection<Task> _availableTasks;
        public ObservableCollection<Task> AvailableTasks
        {
            get { return _availableTasks; }
            set
            {
                if (_availableTasks != value)
                {
                    _availableTasks = value;
                    OnPropertyChanged(() => AvailableTasks);
                }
            }
        }

        private ObservableCollection<string> _availableStateTypes;
        public ObservableCollection<string> AvailableStateTypes
        {
            get
            {
                return _availableStateTypes;
            }
            set
            {
                if (_availableStateTypes != value)
                {
                    _availableStateTypes = value;
                    OnPropertyChanged(() => AvailableStateTypes);
                }
            }
        }
        #endregion

        #region Constructor
        public StatesViewModel()
        {
            _availableStateTypes = new ObservableCollection<string>(Enum.GetNames(typeof(NaoCoopLib.Enums.NaoState)));
        }
        #endregion

        #region Handlers
        protected override void OnRefreshData()
        {
            base.OnRefreshData();
            // refresh tasks
            var tasks = Helpers.DataAccessHelper.Instance.TasksManager.GetRecords();
            AvailableTasks = base.InitializeRecordsCollection<Task, NaoCoopObjects.Classes.Task>(AvailableTasks, tasks);
        }

        protected override void OnSaveData()
        {
            base.OnSaveData();

            // delete pending delete state tasks
            foreach (var state in RecordsCollection)
            {
                foreach (var stateTask in state.PendingDeleteStateTasks)
                {
                    Helpers.DataAccessHelper.Instance.StateTasksManager.DeleteRecord(stateTask.Data);
                }
                state.PendingDeleteStateTasks.Clear();
            }
        }
        #endregion

        
    }
}
