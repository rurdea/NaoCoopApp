using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopDataAccess.Managers
{
    public class ExecutionRobotsManager: RecordsManagerBase<NaoCoopObjects.Classes.ExecutionRobot>
    {
        public ExecutionRobotsManager(string connectionString)
            : base(connectionString)
        {
        }

        protected override System.Data.Linq.ITable Records
        {
            get
            {
                return base.DataContext.GetTable<NaoCoopDataAccess.ExecutionRobot>();
            }
        }

        public override void SaveRecord(NaoCoopObjects.Classes.ExecutionRobot record)
        {
            // save this record
            base.SaveRecord(record);
        }
    }
}
