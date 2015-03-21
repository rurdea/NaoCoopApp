using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaoCoopApp.Validators;

namespace NaoCoopApp.Models
{
    public class Requirement : ModelValidatorBase<NaoCoopObjects.Classes.Requirement>
    {
        #region Constructor
        public Requirement()
            : this(null)
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="robot"></param>
        public Requirement(NaoCoopObjects.Classes.Requirement requirement)
            : base(requirement)
        {
        }
        #endregion

        #region Properties

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a name for the Requirement!")]
        public string Name
        {
            get
            {
                return Data.Name;
            }
            set
            {
                if (Data.Name != value)
                {
                    Data.Name = value;
                    OnPropertyChanged(() => Name);
                }
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a type for the Requirement!")]
        public string Type
        {
            get
            {
                return Data.Type;
            }
            set
            {
                if (Data.Type != value)
                {
                    Data.Type = value;
                    OnPropertyChanged(() => Type);
                }
            }
        }

        #endregion
    }
}
