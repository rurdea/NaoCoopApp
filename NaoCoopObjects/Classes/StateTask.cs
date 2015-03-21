using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NaoCoopObjects.Classes
{
    public class StateTask : NaoCoopObject
    {
        public StateTask()
            : this(Guid.NewGuid())
        {
        }

        public StateTask(Guid id)
            : base(id)
        {
        }

        public Task Task
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
