using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaoCoopApp.Helpers
{
    public class ModelBase<T> : NotificationObject
        where T : NaoCoopObjects.Classes.NaoCoopObject
    {
        #region Properties
        public T Data { get; protected set; }


        public Guid ID
        {
            get
            {
                return Data.ID;
            }
        }
        #endregion

        #region Constructors
        public ModelBase()
            : this(null)
        {
        }

        public ModelBase(T data)
        {
            if (data == null)
            {
                data = Activator.CreateInstance<T>();
            }
            Data = data;
        }
        #endregion
    }
}
