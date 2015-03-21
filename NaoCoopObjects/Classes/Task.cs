using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NaoCoopObjects.Classes
{
    public class Task : NaoCoopObject
    {
        public Task()
            : this(Guid.NewGuid())
        {
        }

        public Task(Guid id)
            : base(id)
        {
            this.Settings = new List<Setting>();
        }

        public string Name
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public List<Setting> Settings
        {
            get;
            private set;
        }
    }
}
