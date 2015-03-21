using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaoCoopApp.Helpers;

namespace NaoCoopApp.Models
{
    public class Page : NotificationObject
    {
        public string Title
        {
            get;
            set;
        }

        public string Source
        {
            get;
            set;
        }
    }
}
