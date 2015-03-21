using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using NaoCoopLib.Types;

namespace NaoCoopLib.Helpers
{
    public class LandMarkHelper
    {
        #region Members
        private static LandMarkHelper _instance;
        #endregion

        #region Properties
        /// <summary>
        /// Singleton implementation
        /// </summary>
        public static LandMarkHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LandMarkHelper();
                }
                return _instance;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Checks if ArrayList is a valid nao mark.
        /// </summary>
        /// <param name="markInfo"></param>
        /// <returns></returns>
        public bool IsValidLandMark(ArrayList landMark)
        {
            return landMark != null &&
                   landMark.Count >= 2 &&
                   landMark[0] is ArrayList &&
                   landMark[1] is ArrayList;
        }

        /// <summary>
        /// Gets the land mark timestamp
        /// </summary>
        /// <param name="markInfo"></param>
        /// <returns></returns>
        public ArrayList GetMarkTimestamp(ArrayList landMarkMemory)
        {
            if (!this.IsValidLandMark(landMarkMemory))
            {
                return null;
            }

            return landMarkMemory[0] as ArrayList;
        }

        public ArrayList GetLandMarksInfo(ArrayList landMarkMemory)
        {
            if (!this.IsValidLandMark(landMarkMemory))
            {
                return null;
            }

            return landMarkMemory[1] as ArrayList;
        }

        /// <summary>
        /// Gets the land mark shape info
        /// </summary>
        /// <param name="markInfo"></param>
        /// <returns></returns>
        public ArrayList GetShapeInfo(ArrayList landMark)
        {
            if (landMark == null || landMark.Count != 2)
            {
                return null;
            }

            return landMark[0] as ArrayList;
        }

        /// <summary>
        /// Gets the land mark ID
        /// </summary>
        /// <param name="markInfo"></param>
        /// <returns></returns>
        public int? GetMarkID(ArrayList landMark)
        {
            if (landMark == null || landMark.Count < 2)
            {
                return null;
            }

            var markExtraInfo = landMark[1] as ArrayList;
            if (markExtraInfo == null || markExtraInfo.Count < 1)
            {
                return null;
            }

            return markExtraInfo[0] as int?;
        }

        /// <summary>
        /// Gets the angle to mark
        /// </summary>
        /// <param name="landMark"></param>
        /// <returns></returns>
        public float? GetAngularSize(ArrayList landMark)
        {
            // Get the shape info
            var shapeInfo = this.GetShapeInfo(landMark);
            if (shapeInfo == null || shapeInfo.Count == 0)
            {
                return null;
            }

            return shapeInfo[3] as float?;
        }

        /// <summary>
        /// Gets the distance to the land mark
        /// </summary>
        /// <param name="markInfo"></param>
        /// <param name="actualLandMarkSize">The actual size of the land mark (in meters).</param>
        /// <returns></returns>
        public float? GetDistanceToLandMark(ArrayList landMark, float actualLandMarkSize)
        {
            // Retrieve landmark angular size in radians.
            var angularSize = GetAngularSize(landMark);

            // Return if could not calculate the angular size
            if (!angularSize.HasValue)
            {
                return null;
            }

            // Compute distance to landmark.
            var distanceFromCameraToLandmark = actualLandMarkSize / (2 * Math.Tan(angularSize.Value / 2));

            return (float)distanceFromCameraToLandmark;
        }

        /// <summary>
        /// Gets the rotation to point towards landmark
        /// </summary>
        /// <param name="landMark"></param>
        /// <returns></returns>
        public Transform GetLandMarkRotationTransform(ArrayList landMark)
        {
            // Get the shape info
            var shapeInfo = this.GetShapeInfo(landMark);
            if (shapeInfo == null || shapeInfo.Count == 0)
            {
                return null;
            }

            // Retrieve landmark center position in radians.
            var wzCamera = shapeInfo[1] as float?;
            var wyCamera = shapeInfo[1] as float?;

            if (!wyCamera.HasValue || !wzCamera.HasValue)
            {
                return null;
            }

            // Compute the rotation to point towards the landmark.
            var cameraToLandmarkRotationTransform = ALMathHelper.TransformFrom3DRotation(0f, wyCamera.Value, wzCamera.Value);

            return cameraToLandmarkRotationTransform;
        }

        /// <summary>
        /// Returns the transaltion from camera to land mark
        /// </summary>
        /// <param name="landMark"></param>
        /// <returns></returns>
        public Transform GetCameraToLandMarkTranslationTransform(ArrayList landMark, float actualLandMarkSize)
        { 
            var distanceFromCameraToLandmark = this.GetDistanceToLandMark(landMark, actualLandMarkSize);
            if (!distanceFromCameraToLandmark.HasValue)
            {
                return null;
            }
            // Compute the translation to reach the landmark.
            var cameraToLandmarkTranslationTransform = new Transform(distanceFromCameraToLandmark.Value, 0f, 0f);

            return cameraToLandmarkTranslationTransform;
        }

        public NaoPosition GetLandMarkPosition(ArrayList landMark, float actualLandMarkSize, Transform robotToCamera)
        {
            if (robotToCamera == null)
            {
                return null;
            }

            var cameraToLandmarkRotationTransform = this.GetLandMarkRotationTransform(landMark);
            var cameraToLandmarkTranslationTransform = this.GetCameraToLandMarkTranslationTransform(landMark, actualLandMarkSize);

            if (cameraToLandmarkRotationTransform == null || cameraToLandmarkTranslationTransform == null)
            {
                return null;
            }
            // Combine all transformations to get the landmark position in NAO space.
            var robotToLandmark = robotToCamera * cameraToLandmarkRotationTransform * cameraToLandmarkTranslationTransform;

            return new NaoPosition(robotToLandmark.r1_c4, robotToLandmark.r2_c4, robotToLandmark.r3_c4);
        }
        #endregion
    }
}
