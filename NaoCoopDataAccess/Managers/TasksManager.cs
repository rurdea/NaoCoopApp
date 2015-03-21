using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopDataAccess.Managers
{
    public class TasksManager : RecordsManagerBase<NaoCoopObjects.Classes.Task>
    {
        #region Members
        private SettingsManager _settingsManager;
        #endregion

        public TasksManager(string connectionString)
            : base(connectionString)
        {
            _settingsManager = new SettingsManager(connectionString);
        }

        protected override System.Data.Linq.ITable Records
        {
            get 
            {
                return base.DataContext.GetTable<NaoCoopDataAccess.Task>(); 
            }
        }

        public override void SaveRecord(NaoCoopObjects.Classes.Task record)
        {
            // save this record
            base.SaveRecord(record);
            // save the task settings
            foreach (var setting in record.Settings)
            {
                _settingsManager.SaveRecord((NaoCoopObjects.Classes.Setting)setting, record);
            }
        }
    }
}
