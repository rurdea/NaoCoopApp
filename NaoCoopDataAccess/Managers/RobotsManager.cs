using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopDataAccess.Managers
{
    public class RobotsManager: RecordsManagerBase<NaoCoopObjects.Classes.Robot>
    {
        public RobotsManager(string connectionString)
            : base(connectionString)
        {
        }

        protected override System.Data.Linq.ITable Records
        {
            get
            {
                return base.DataContext.GetTable<NaoCoopDataAccess.Robot>();
            }
        }

        public override void SaveRecord(NaoCoopObjects.Classes.Robot record)
        {
            // save this record
            base.SaveRecord(record);
        }
    }
}
