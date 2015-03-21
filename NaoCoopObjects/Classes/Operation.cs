using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopObjects.Classes
{
    public class Operation : NaoCoopObject
    {
        public Operation()
            : this(Guid.NewGuid())
        {
        }

        public Operation(Guid id)
            : base(id)
        {
            this.OperationRobots = new List<OperationRobot>();
            this.OperationRequirements = new List<OperationRequirement>();
        }

        public string Name
        {
            get;
            set;
        }

        public List<OperationRobot> OperationRobots
        {
            get;
            private set;
        }

        public List<OperationRequirement> OperationRequirements
        {
            get;
            private set;
        }
    }
}
