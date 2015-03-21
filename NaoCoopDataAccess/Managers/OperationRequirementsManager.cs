using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopDataAccess.Managers
{
    public class OperationRequirementsManager: RecordsManagerBase<NaoCoopObjects.Classes.OperationRequirement>
    {
        public OperationRequirementsManager(string connectionString)
            : base(connectionString)
        {
        }

        protected override System.Data.Linq.ITable Records
        {
            get { return base.DataContext.GetTable<NaoCoopDataAccess.OperationRequirement>(); }
        }

        public override void SaveRecord(NaoCoopObjects.Classes.OperationRequirement record)
        {
            base.SaveRecord(record);
        }
    }
}
