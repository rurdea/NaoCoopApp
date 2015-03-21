using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopObjects.Classes
{
    public class RobotVersion : NaoCoopObject
    {
        public RobotVersion()
            : this(Guid.NewGuid())
        {
        }

        public RobotVersion(Guid id)
            : base(id)
        {
        }

        public string Name
        {
            get;
            set;
        }
    }
}
