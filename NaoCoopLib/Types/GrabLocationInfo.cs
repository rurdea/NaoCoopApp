using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NaoCoopLib.Enums;
using NaoCoopLib.Executers;

namespace NaoCoopLib.Types
{
    public class GrabLocationInfo
    {
        #region Constants
        protected const float DEFAULT_GRAB_LOCATION_X = 0.25f;
        protected const float DEFAULT_GRAB_LOCATION_Y = 0.35f;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the grab location
        /// </summary>
        public GrabLocation GrabLocation
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the position of the grab location (relative with the checkpoint position).
        /// </summary>
        public NaoPosition Position
        {
            get;
            private set;
        }
        #endregion

        #region Constructors
        public GrabLocationInfo(GrabLocation grabLocation)
            : this (grabLocation, null)
        {

        }

        public GrabLocationInfo(GrabLocation grabLocation, NaoPosition position)
        {
            this.GrabLocation = grabLocation;

            if (position != null)
            {
                this.Position = position;
            }
            else
            {
                this.Position = new NaoPosition(DEFAULT_GRAB_LOCATION_X, grabLocation == GrabLocation.A ? DEFAULT_GRAB_LOCATION_Y : -DEFAULT_GRAB_LOCATION_Y, 0f);
            }
        }
        #endregion
    }
}