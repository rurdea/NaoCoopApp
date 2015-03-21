using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopObjects.Classes
{
    public class ExecutionRobot : NaoCoopObject
    {
        public ExecutionRobot()
            : this(Guid.NewGuid())
        {
        }

        public ExecutionRobot(Guid id)
            : base(id)
        {
        }

        public Robot Robot
        {
            get;
            set;
        }
    }
}
