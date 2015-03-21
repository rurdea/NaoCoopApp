using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaoCoopApp.Models
{
    public class RequirementInput : Requirement
    {
        #region Constructor
        public RequirementInput()
            : this(null)
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="robot"></param>
        public RequirementInput(NaoCoopObjects.Classes.Requirement requirement)
            : base(requirement)
        {
        }
        #endregion

        private string _value;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a value for the Requirement!")]
        public string InputValue
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged(() => InputValue);
                }
            }
        }

        private string _typeDescription;
        public string TypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(_typeDescription))
                {
                    if (this.Type == NaoCoopObjects.Classes.RequirementType.@bool.ToString())
                    {
                        _typeDescription = "'true' or 'false'";
                    }
                    else if (this.Type == NaoCoopObjects.Classes.RequirementType.@double.ToString())
                    {
                        _typeDescription = "number";
                    }
                }

                return _typeDescription;
            }
        }
    }
}
