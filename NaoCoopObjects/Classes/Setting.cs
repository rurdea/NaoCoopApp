using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NaoCoopObjects.Classes
{
    public class Setting : NaoCoopObject
    {
        public Setting()
            : this(Guid.NewGuid())
        {
        }

        public Setting(Guid id)
            : base(id)
        {
        }

        public string Name
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }
    }
}
