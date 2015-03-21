using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopDataAccess.Managers
{
    public class RequirementsManager : RecordsManagerBase<NaoCoopObjects.Classes.Requirement>
    {
        public RequirementsManager(string connectionString)
            : base(connectionString)
        {
        }

        protected override System.Data.Linq.ITable Records
        {
            get
            {
                return base.DataContext.GetTable<NaoCoopDataAccess.Requirement>();
            }
        }

        public override void SaveRecord(NaoCoopObjects.Classes.Requirement record)
        {
            // save this record
            base.SaveRecord(record);
        }
    }
}
