using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopObjects.Classes
{

    public enum OperationValueValidator
    {
        Equal,
        NotEqual,
        GreaterThan,
        LessThan
    }

    public class OperationRequirement : NaoCoopObject
    {
        public OperationRequirement()
            : this(Guid.NewGuid())
        {
        }

        public OperationRequirement(Guid id)
            : base(id)
        {
        }

        public string Value
        {
            get;
            set;
        }

        public string ValueValidator
        {
            get;
            set;
        }

        public Requirement Requirement
        {
            get;
            set;
        }
    }
}
