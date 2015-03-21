using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopLib.Types
{
    public class WalkToLandMarkInfo : LandMarkInfo
    {
        #region Constants
        private const float DEFAULT_STOP_DISTANCE = 0.2f;
        private const float DEFAULT_ADVANCE_DISTANCE = 0.2f;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the distance where the Nao should stop before the mark
        /// </summary>
        public float StopDistance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the distance to advance towards the nao mark
        /// </summary>
        public float AdvanceDistance
        {
            get;
            private set;
        }
        #endregion

        #region Constructor
        public WalkToLandMarkInfo(int markID, float markSize, float stopDistance = DEFAULT_STOP_DISTANCE, float advanceDistance = DEFAULT_ADVANCE_DISTANCE)
            : base(markID, markSize)
        {
            this.StopDistance = stopDistance;
            this.AdvanceDistance = advanceDistance;
        }
        #endregion
    }
}
