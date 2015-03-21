using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopDataAccess.Managers
{
    public class SettingsManager : RecordsManagerBase<NaoCoopObjects.Classes.Setting>
    {
        public SettingsManager(string connectionString)
            : base(connectionString)
        {
        }

        protected override System.Data.Linq.ITable Records
        {
            get 
            {
                return base.DataContext.GetTable<NaoCoopDataAccess.Setting>(); 
            }
        }

        public override void SaveRecord(NaoCoopObjects.Classes.Setting record)
        {
            // save this record
            base.SaveRecord(record);
        }
    }
}
