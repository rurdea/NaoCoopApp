using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaoCoopApp.Helpers;

namespace NaoCoopApp.Models
{
    public class OperationRobotState : ModelBase<NaoCoopObjects.Classes.OperationRobotState>
    {
        #region Constructors
        public OperationRobotState()
            : this (null)
        {
        }

        public OperationRobotState(NaoCoopObjects.Classes.OperationRobotState robotState)
            : base(robotState)
        {
        }
        #endregion

        private State _state;
        public State State
        {
            get
            {
                if (_state == null)
                {
                    _state = new State(Data.State);
                }
                return _state;
            }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    Data.State = value.Data;
                    OnPropertyChanged(() => State);
                }
            }
        }

        public int Order
        {
            get
            {
                return Data.Order;
            }
            set
            {
                if (value != Data.Order)
                {
                    Data.Order = value;
                    OnPropertyChanged(() => Order);
                }
            }
        }
    }
}
