using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaoCoopApp.Helpers;
using NaoCoopApp.Validators;

namespace NaoCoopApp.Models
{
    public class Setting : ModelValidatorBase<NaoCoopObjects.Classes.Setting>
    {
        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public Setting()
            : this(null)
        {
        }

        /// <summary>
        /// Constructor taking a nao setting object as parameter
        /// </summary>
        /// <param name="robot"></param>
        public Setting(NaoCoopObjects.Classes.Setting setting)
            : base(setting)
        {
        }
        #endregion

        #region Properties
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a name for the Setting!")]
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

        public string Value
        {
            get
            {
                return Data.Value;
            }
            set
            {
                if (Data.Value != value)
                {
                    Data.Value = value;
                    OnPropertyChanged(() => Value);
                }
            }
        }
        #endregion
    }
}
