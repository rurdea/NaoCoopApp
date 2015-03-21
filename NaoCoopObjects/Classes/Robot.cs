using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NaoCoopObjects.Classes
{
    public class Robot : NaoCoopObject
    {
        public Robot()
            : this(Guid.NewGuid())
        {
        }

        public Robot(Guid id)
            : base(id)
        {
        }

        public string Name
        {
            get;
            set;
        }

        public string IP
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public bool Enabled
        {
            get;
            set;
        }

        public RobotVersion RobotVersion
        {
            get;
            set;
        }
    }
}
