using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NaoCoopObjects.Classes
{
    public class OperationRobotState : NaoCoopObject
    {
        public OperationRobotState()
            : this(Guid.NewGuid())
        {
        }

        public OperationRobotState(Guid id)
            : base(id)
        {
        }

        public State State
        {
            get;
            set;
        }

        public int Order
        {
            get;
            set;
        }
    }
}
