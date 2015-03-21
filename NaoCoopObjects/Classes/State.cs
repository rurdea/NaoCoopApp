using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NaoCoopObjects.Classes
{
    public class State : NaoCoopObject
    {
        public State()
            : this(Guid.NewGuid())
        {
        }

        public State(Guid id)
            : base(id)
        {
            this.StateTasks = new List<StateTask>();
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

        public List<StateTask> StateTasks
        {
            get;
            private set;
        }
    }
}
