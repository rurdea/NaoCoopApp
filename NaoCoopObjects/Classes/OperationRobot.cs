using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopObjects.Classes
{
    public class OperationRobot : NaoCoopObject
    {
        public OperationRobot()
            : this(Guid.NewGuid())
        {
        }

        public OperationRobot(Guid id)
            : base(id)
        {
            this.OperationRobotStates = new List<OperationRobotState>();
        }

        public string Name
        {
            get;
            set;
        }

        public RobotVersion RobotVersion
        {
            get;
            set;
        }

        public List<OperationRobotState> OperationRobotStates
        {
            get;
            private set;
        }
    }
}
