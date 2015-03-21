using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaoCoopApp.Validators;

namespace NaoCoopApp.Models
{
    public class RobotVersion : ModelValidatorBase<NaoCoopObjects.Classes.RobotVersion>
    {
         #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public RobotVersion()
            : this(null)
        {
        }

        /// <summary>
        /// Constructor taking a nao robot object as parameter
        /// </summary>
        /// <param name="robotVersion"></param>
        public RobotVersion(NaoCoopObjects.Classes.RobotVersion robotVersion)
            : base(robotVersion)
        {
        }
        #endregion

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a name for the Robot Version!")]
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
    }
}
