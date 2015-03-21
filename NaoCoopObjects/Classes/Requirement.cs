using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopObjects.Classes
{
    public enum RequirementType
    {
        @bool,
        @double
    }

    public class Requirement: NaoCoopObject
    {
        public Requirement()
            : this(Guid.NewGuid())
        {
        }

        public Requirement(Guid id)
            : base(id)
        {
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
    }
}
