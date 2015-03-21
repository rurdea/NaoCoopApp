using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaoCoopApp.Helpers;

namespace NaoCoopApp.Models
{
    public class OperationRequirement : ModelBase<NaoCoopObjects.Classes.OperationRequirement>
    {
        #region Constructors
        public OperationRequirement()
        {
        }

        public OperationRequirement(NaoCoopObjects.Classes.OperationRequirement operationrequirement)
            : base(operationrequirement)
        {
        }
        #endregion
        private Requirement _requirement;
        public Requirement Requirement
        {
            get
            {
                if (_requirement == null)
                {
                    _requirement = new Requirement(Data.Requirement);
                }
                return _requirement;
            }
            set
            {
                if (_requirement != value)
                {
                    _requirement = value;
                    Data.Requirement = value.Data;
                    OnPropertyChanged(() => Requirement);
                }
            }
        }

        public string Value
        {
            get
            {
                return Data.Value;
            }
            set
            {
                if (value != Data.Value)
                {
                    Data.Value = value;
                    OnPropertyChanged(() => Value);
                }
            }
        }

        public string ValueValidator
        {
            get
            {
                return Data.ValueValidator;
            }
            set
            {
                if (value != Data.ValueValidator)
                {
                    Data.ValueValidator = value;
                    OnPropertyChanged(() => ValueValidator);
                }
            }
        }
    }
}