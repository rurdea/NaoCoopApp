using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopDataAccess.Managers
{
    public class RobotVersionsManager : RecordsManagerBase<NaoCoopObjects.Classes.RobotVersion>
    {
        public RobotVersionsManager(string connectionString)
            : base(connectionString)
        {
        }

        protected override System.Data.Linq.ITable Records
        {
            get
            {
                return base.DataContext.GetTable<NaoCoopDataAccess.RobotVersion>();
            }
        }

        public override void SaveRecord(NaoCoopObjects.Classes.RobotVersion record)
        {
            // save this record
            base.SaveRecord(record);
        }
    }
}
