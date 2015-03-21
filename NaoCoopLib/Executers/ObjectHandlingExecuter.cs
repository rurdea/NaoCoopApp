using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NaoCoopLib.Helpers;
using Aldebaran.Proxies;
using NaoCoopLib.Types;

namespace NaoCoopLib.Executers
{
    /// <summary>
    /// Object handling executor
    /// </summary>
    public class ObjectHandlingExecuter : ExecuterBase
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public ObjectHandlingExecuter(string ip, int port) : base (ip, port)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection"></param>
        public ObjectHandlingExecuter(NaoConnectionHelper connection)
            : base(connection)
        {

        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Makes the robot move from the checkpoint to the specified grab location (A,B)
        /// </summary>
        /// <param name="grabLocationInfo"></param>
        public void GoToGrabLocation(GrabLocationInfo grabLocationInfo)
        {
            using (var motionProxy = _connection.GetProxy<MotionProxy>())
            {
                motionProxy.moveTo(grabLocationInfo.Position.x, grabLocationInfo.Position.y, grabLocationInfo.Position.z);
            }
        }

        /// <summary>
        /// Makes the robot go into the lift position
        /// </summary>
        public void GoToLiftPosition()
        {
            InitHands();
            Sit();
            LiftReady();
        }

        /// <summary>
        /// Makes the robot go into the stand position
        /// </summary>
        public void StandUp()
        {
            using (var motionProxy = _connection.GetProxy<MotionProxy>())
            {
                motionProxy.setWalkArmsEnabled(false, false);
                motionProxy.walkInit();
                motionProxy.setWalkArmsEnabled(true, true);
            }
        }

        /// <summary>
        /// Makes the robot lift the object
        /// </summary>
        public void LiftObject()
        {
            this.Lift();
        }

        /// <summary>
        /// Starts robot walking
        /// </summary>
        public void WalkWithObject()
        {
            using (var motionProxy = _connection.GetProxy<MotionProxy>())
            {
                motionProxy.setWalkArmsEnabled(false, false);
                motionProxy.walkInit();
                motionProxy.moveTo(0.5f, 0f, 0f);
                motionProxy.setWalkArmsEnabled(true, true);
            }
        }

        /// <summary>
        /// Stops walking
        /// </summary>
        public void StopWalking()
        {
            using (var motionProxy = _connection.GetProxy<MotionProxy>())
            {
                motionProxy.stopMove();
            }
        }


        /// <summary>
        /// Disposes current object
        /// </summary>
        public override void Dispose()
        {
        }
        #endregion

        #region Private
        private void InitHands()
        {
            List<string> names = new List<string>();
            List<float[]> times = new List<float[]>();
            List<float[]> keys = new List<float[]>();

            var time = 1.5f;
            var elbowRoll = MotionHelper.DegreeToRadian(20f);

            #region 1
            names.Add("LElbowRoll");
            times.Add(new[] { time });
            keys.Add(new[] { elbowRoll });

            names.Add("RElbowRoll");
            times.Add(new[] { time });
            keys.Add(new[] { -elbowRoll });
            #endregion

            AngleInterpolation(names, keys, times, true);
        }

        private void Sit()
        {
            List<string> names = new List<string>();
            List<float[]> times = new List<float[]>();
            List<float[]> keys = new List<float[]>();

            var time = 2.5f;
            var hipYawPitch = 0f;
            var hipRoll = 0f;
            var hipPitch = -0.4361111f;
            var kneePitch = 1.6572222f;
            var anklePitch = -1.1862222f;
            var ankleRoll = 0f;

            #region Positions

            names.Add("LHipYawPitch");
            times.Add(new[] { time });
            keys.Add(new[] { hipYawPitch });

            names.Add("RHipYawPitch");
            times.Add(new[] { time });
            keys.Add(new[] { hipYawPitch });

            names.Add("LHipRoll");
            times.Add(new[] { time });
            keys.Add(new[] { hipRoll });

            names.Add("RHipRoll");
            times.Add(new[] { time });
            keys.Add(new[] { hipRoll });

            names.Add("LHipPitch");
            times.Add(new[] { time });
            keys.Add(new[] { hipPitch });

            names.Add("RHipPitch");
            times.Add(new[] { time });
            keys.Add(new[] { hipPitch });

            names.Add("LKneePitch");
            times.Add(new[] { time });
            keys.Add(new[] { kneePitch });

            names.Add("RKneePitch");
            times.Add(new[] { time });
            keys.Add(new[] { kneePitch });

            names.Add("LAnklePitch");
            times.Add(new[] { time });
            keys.Add(new[] { anklePitch });

            names.Add("RAnklePitch");
            times.Add(new[] { time });
            keys.Add(new[] { anklePitch });

            names.Add("LAnkleRoll");
            times.Add(new[] { time });
            keys.Add(new[] { ankleRoll });

            names.Add("RAnkleRoll");
            times.Add(new[] { time });
            keys.Add(new[] { ankleRoll });
            #endregion

            AngleInterpolation(names, keys, times, true);
        }

        private void LiftReady()
        {
            List<string> names = new List<string>();
            List<float[]> times = new List<float[]>();
            List<float[]> keys = new List<float[]>();

            var time = 1.5f;
            var shoulderPitch = 1.3659f;
            var shoulderRoll = 0.12734f;
            var elbowYaw = -1.440911f;
            var elbowRoll = -0.96816f;
            var wristYaw = -1.814222f;
            var hand = 0f;

            #region Positions
            names.Add("LShoulderPitch");
            times.Add(new[] { time });
            keys.Add(new[] { shoulderPitch });

            names.Add("RShoulderPitch");
            times.Add(new[] { time });
            keys.Add(new[] { shoulderPitch });

            names.Add("LShoulderRoll");
            times.Add(new[] { time });
            keys.Add(new[] { shoulderRoll });

            names.Add("RShoulderRoll");
            times.Add(new[] { time });
            keys.Add(new[] { -shoulderRoll });

            names.Add("LElbowYaw");
            times.Add(new[] { time });
            keys.Add(new[] { elbowYaw });

            names.Add("RElbowYaw");
            times.Add(new[] { time });
            keys.Add(new[] { -elbowYaw });

            names.Add("LElbowRoll");
            times.Add(new[] { time });
            keys.Add(new[] { elbowRoll });

            names.Add("RElbowRoll");
            times.Add(new[] { time });
            keys.Add(new[] { -elbowRoll });

            names.Add("LWristYaw");
            times.Add(new[] { time });
            keys.Add(new[] { wristYaw });

            names.Add("RWristYaw");
            times.Add(new[] { time });
            keys.Add(new[] { -wristYaw });

            names.Add("LHand");
            times.Add(new[] { time });
            keys.Add(new[] { hand });

            names.Add("RHand");
            times.Add(new[] { time });
            keys.Add(new[] { hand });
            #endregion

            AngleInterpolation(names, keys, times, true);
        }

        private void Lift()
        {
            List<string> names = new List<string>();
            List<float[]> times = new List<float[]>();
            List<float[]> keys = new List<float[]>();

            var time = 1.5f;
            var shoulderPitch = MotionHelper.DegreeToRadian(40f);

            #region Positions
            names.Add("LShoulderPitch");
            times.Add(new[] { time });
            keys.Add(new[] { shoulderPitch });

            names.Add("RShoulderPitch");
            times.Add(new[] { time });
            keys.Add(new[] { shoulderPitch });
            #endregion

            AngleInterpolation(names, keys, times, true);
        }


        private void AngleInterpolation(List<string> names, List<float[]> keys, List<float[]> times, bool isAbsolute)
        {
            using (var motionProxy = _connection.GetProxy<MotionProxy>())
            {
                motionProxy.angleInterpolation(names, keys, times, isAbsolute);
            }
        }
        #endregion
    }
}
