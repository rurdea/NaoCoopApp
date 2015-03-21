using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopDataAccess.Managers
{
    public class UsersManager: RecordsManagerBase<NaoCoopObjects.Classes.User>
    {
        public UsersManager(string connectionString)
            : base(connectionString)
        {
        }

        protected override System.Data.Linq.ITable Records
        {
            get
            {
                return base.DataContext.GetTable<NaoCoopDataAccess.User>();
            }
        }

        public override void SaveRecord(NaoCoopObjects.Classes.User record)
        {
            // save this record
            base.SaveRecord(record);
        }

        public NaoCoopObjects.Classes.User ValidateUsernameAndPassword(string username, string password)
        {
            User user = null;

            // find user
            foreach (NaoCoopDataAccess.User record in Records)
            {
                if (record.Username.ToLower().Equals(username.ToLower()) && record.Password.Equals(password))
                {
                    user = record;
                    break;
                }
            }

            return (NaoCoopObjects.Classes.User)ConvertDbObjToNaoObj(user, typeof(NaoCoopObjects.Classes.User));
        }
    }
}
