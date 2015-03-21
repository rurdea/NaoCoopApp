using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NaoCoopObjects.Interfaces;


namespace NaoCoopObjects.Classes
{
    public class NaoCoopObject : INaoCoopObject
    {
        #region Constructors
        public NaoCoopObject(Guid id)
        {
            this.ID = id;

            // instantiate all lists
        }
        #endregion

        #region Properties
        public Guid ID
        {
            get;
            private set;
        }
        #endregion

        public override bool Equals(object obj)
        {
            if (obj is NaoCoopObject)
            {
                return this.ID.Equals(((NaoCoopObject)obj).ID);
            }
            else
            {
                return base.Equals(obj);
            }
        }
    }
}
