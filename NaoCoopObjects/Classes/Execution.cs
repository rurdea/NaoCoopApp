using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopObjects.Classes
{
    public enum ExecutionStatus
    {
        Pending,
        Started,
        Paused,
        Completed,
        Failed
    }

    public class Execution: NaoCoopObject
    {
        public Execution()
            : this(Guid.NewGuid())
        {
        }

        public Execution(Guid id)
            : base(id)
        {
            ExecutionRobots = new List<ExecutionRobot>();
        }

        public DateTime DateCreated
        {
            get;
            set;
        }

        public DateTime DateStarted
        {
            get;
            set;
        }

        public DateTime DateEnded
        {
            get;
            set;
        }

        public ExecutionStatus Status
        {
            get;
            set;
        }

        public Operation Operation
        {
            get;
            set;
        }

        public List<ExecutionRobot> ExecutionRobots
        {
            get;
            private set;
        }
    }
}
