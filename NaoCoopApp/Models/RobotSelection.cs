using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaoCoopApp.Models
{
    public class RobotSelection : Robot
    {
         #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public RobotSelection()
            : this(null)
        {
        }

        /// <summary>
        /// Constructor taking a nao robot object as parameter
        /// </summary>
        /// <param name="robot"></param>
        public RobotSelection(NaoCoopObjects.Classes.Robot robot)
            : base(robot)
        {
        }
        #endregion

        private bool _isSelected;
        public bool Selected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(() => Selected);
                }
            }
        }
    }
}
