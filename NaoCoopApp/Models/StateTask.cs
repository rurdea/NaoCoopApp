using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaoCoopApp.Helpers;

namespace NaoCoopApp.Models
{
    public class StateTask : ModelBase<NaoCoopObjects.Classes.StateTask>
    {
        #region Constructors
        public StateTask()
            : this (null)
        {
        }

        public StateTask(NaoCoopObjects.Classes.StateTask stateTask)
            : base(stateTask)
        {

        }
        #endregion

        private Task _task;
        public Task Task
        {
            get
            {
                if (_task == null)
                {
                    _task = new Task(Data.Task);
                }
                return _task;
            }
            set
            {
                if (_task != value)
                {
                    _task = value;
                    Data.Task = value.Data;
                    OnPropertyChanged(() => Task);
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
