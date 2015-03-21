using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaoCoopApp.Validators;

namespace NaoCoopApp.Models
{
    public class ExecutionRobot : ModelValidatorBase<NaoCoopObjects.Classes.ExecutionRobot>
    {
        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public ExecutionRobot()
        {
        }

        /// <summary>
        /// Constructor taking a nao execution robot object as parameter
        /// </summary>
        /// <param name="execution"></param>
        public ExecutionRobot(NaoCoopObjects.Classes.ExecutionRobot executionRobot)
            : base(executionRobot)
        {
        }
        #endregion

        #region Properties
        private Robot _robot;
        public Robot Robot
        {
            get
            {
                if (_robot == null)
                {
                    _robot = new Robot(Data.Robot);
                }
                return _robot;
            }
            set
            {
                if (_robot != value)
                {
                    _robot = value;
                    Data.Robot = value.Data;
                    OnPropertyChanged(() => Robot);
                }
            }
        }

        private string _currentState;
        public string CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                if (_currentState != value)
                {
                    _currentState = value;
                    OnPropertyChanged(() => CurrentState);
                }
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(() => Status);
                }
            }
        }

        private string _currentCommand;
        public string CurrentCommand
        {
            get
            {
                return _currentCommand;
            }
            set
            {
                if (_currentCommand != value)
                {
                    _currentCommand = value;
                    OnPropertyChanged(() => CurrentCommand);
                }
            }
        }
        #endregion
    }
}
