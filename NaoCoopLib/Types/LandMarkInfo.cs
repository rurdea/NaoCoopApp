using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopLib.Types
{
    public class LandMarkInfo
    {
        public int MarkID
        {
            get;
            private set;
        }

        public float MarkSize
        {
            get;
            private set;
        }

        public LandMarkInfo(int markID, float markSize)
        {
            this.MarkID = markID;
            this.MarkSize = markSize;
        }
    }
}
