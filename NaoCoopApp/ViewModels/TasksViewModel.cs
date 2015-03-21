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
    public class TasksViewModel : RecordsViewModel<Task, NaoCoopObjects.Classes.Task>
    {
        #region Properties
        private ObservableCollection<string> _availableTaskTypes;
        public ObservableCollection<string> AvailableTaskTypes
        {
            get
            {
                return _availableTaskTypes;
            }
            set
            {
                if (_availableTaskTypes != value)
                {
                    _availableTaskTypes = value;
                    OnPropertyChanged(() => AvailableTaskTypes);
                }
            }
        }
        #endregion

        #region Constructor
        public TasksViewModel()
        {
            _availableTaskTypes = new ObservableCollection<string>(Enum.GetNames(typeof(NaoCoopLib.Enums.NaoCommand)));
        }
        #endregion

        #region Handlers
        protected override void OnSaveData()
        {
            base.OnSaveData();

            // delete pending delete settings
            foreach (var task in RecordsCollection)
            {
                foreach (var setting in task.PendingDeleteSettings)
                {
                    Helpers.DataAccessHelper.Instance.SettingsManager.DeleteRecord(setting.Data);
                }
                task.PendingDeleteSettings.Clear();
            }
        }
        #endregion
    }
}
