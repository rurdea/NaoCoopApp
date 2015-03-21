using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaoCoopApp.Validators;

namespace NaoCoopApp.Models
{
    public class Robot : ModelValidatorBase<NaoCoopObjects.Classes.Robot>
    {
        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public Robot()
            : this(null)
        {
        }

        /// <summary>
        /// Constructor taking a nao robot object as parameter
        /// </summary>
        /// <param name="robot"></param>
        public Robot(NaoCoopObjects.Classes.Robot robot)
            : base(robot)
        {
        }
        #endregion

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a name for the Robot!")]
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

        [RegularExpression(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$", ErrorMessage = "Please provide a valid V4 IP address for the Robot!")]
        public string IP
        {
            get
            {
                return Data.IP;
            }
            set
            {
                if (Data.IP != value)
                {
                    Data.IP = value;
                    OnPropertyChanged(() => IP);
                }
            }
        }

        public int Port
        {
            get
            {
                return Data.Port;
            }
            set
            {
                if (Data.Port != value)
                {
                    Data.Port = value;
                    OnPropertyChanged(() => Port);
                }
            }
        }

        public bool Enabled
        {
            get
            {
                return Data.Enabled;
            }
            set
            {
                if (Data.Enabled != value)
                {
                    Data.Enabled = value;
                    OnPropertyChanged(() => Enabled);
                }
            }
        }

        private RobotVersion _robotVersion;
        public RobotVersion RobotVersion
        {
            get
            {
                if (_robotVersion == null)
                {
                    _robotVersion = new RobotVersion(Data.RobotVersion);
                }
                return _robotVersion;
            }
            set
            {
                if (_robotVersion != value)
                {
                    _robotVersion = value;
                    Data.RobotVersion = value.Data;
                    OnPropertyChanged(() => RobotVersion);
                }
            }
        }
    }
}
