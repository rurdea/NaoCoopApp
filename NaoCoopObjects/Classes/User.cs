using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopObjects.Classes
{
    public class User : NaoCoopObject
    {
        public User()
            : this(Guid.NewGuid())
        {
        }

        public User(Guid id)
            : base(id)
        {

        }

        public string FullName
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public bool IsAdmin
        {
            get;
            set;
        }
    }
}
