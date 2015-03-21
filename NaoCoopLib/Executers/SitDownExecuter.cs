using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aldebaran.Proxies;
using NaoCoopLib.Enums;

namespace NaoCoopLib.Executers
{
    public class SitDownExecuter
    {
        MotionProxy motionProxy;
        RobotPoseProxy robotPoseProxy;

        public SitDownExecuter(MotionProxy motionProxy, RobotPoseProxy robotPoseProxy)
        {
            this.motionProxy = motionProxy;
            this.robotPoseProxy = robotPoseProxy;
        }

        public bool Start()
        {

            NAOPositions curPosition = GetPosition();
            if (curPosition == NAOPositions.Unknown || curPosition == NAOPositions.HeadBack)
            {
                return false;
            }

            motionProxy.stiffnessInterpolation(NAOBodyParts.Body.ToString(), 1, 1);
            while (curPosition != NAOPositions.Sit)
            {
                switch (curPosition)
                {
                    case NAOPositions.Belly:
                    case NAOPositions.Frog:
                    case NAOPositions.Knee:
                        ChangePositionFromBellyToCrouch();
                        break;
                    case NAOPositions.Back:
                        ChangePositionFromBackToSitDown();
                        break;
                    case NAOPositions.Right:
                    case NAOPositions.Left:
                        ChangePositionFromSideToBelly();
                        break;
                    case NAOPositions.Stand:
                    case NAOPositions.Crouch:
                        ChangePositionFromCrouchToSitDown();
                        break;
                    case NAOPositions.HeadBack:
                        return false;
                    case NAOPositions.Unknown:
                        return false;
                    default:
                        return false;
                }
                curPosition = GetPosition();
            }
            //motionProxy.stiffnessInterpolation(NAOBodyParts.Body.ToString(), 0, 1);//Release stiffness
            return true;
        }

        private void ChangePositionFromCrouchToSitDown()
        {
            List<string> names = new List<string>();
            List<float[]> times = new List<float[]>();
            List<float[]> keys = new List<float[]>();

            names.Add("HeadYaw");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 4.30000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { -0.31605f, -0.44030f, -0.63358f, -0.62745f, -0.40962f, -0.18412f, -0.00004f, -0.02152f });

            names.Add("HeadPitch");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 4.30000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { 0.37119f, 0.37579f, 0.36812f, 0.35738f, 0.36812f, 0.36658f, 0.37886f, -0.03379f });

            names.Add("LShoulderPitch");
            times.Add(new[] { 1.30000f, 2.40000f, 2.90000f, 3.40000f, 4.30000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { 1.04371f, 0.72955f, 0.72955f, 1.01086f, 1.50481f, 2.03404f, 1.53764f, 0.67719f, 0.91269f });

            names.Add("LShoulderRoll");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 4.30000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { 0.33590f, 0.53532f, 0.48171f, 0.83776f, 0.44331f, 1.55683f, 0.56907f, 0.21932f });

            names.Add("LElbowYaw");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 4.30000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { -0.56302f, -0.08552f, 0.10580f, -1.55334f, -0.32985f, -1.20777f, -0.76091f, -0.43723f });

            names.Add("LElbowRoll");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 4.30000f, 4.70000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { -0.95871f, -1.20253f, -0.69813f, -0.90234f, -0.48695f, -0.68591f, -0.95993f, -1.32227f, -1.28085f });

            names.Add("LWristYaw");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 4.30000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { 0.08740f, -0.71642f, -0.71642f, -0.71642f, -0.72869f, -0.73023f, -0.72256f, 0.05978f });

            names.Add("LHand");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 4.30000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { 0.00404f, 0.00117f, 0.00116f, 0.00116f, 0.00128f, 0.00121f, 0.00106f, 0.00524f });

            names.Add("RShoulderPitch");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 3.90000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { 0.80809f, 0.59341f, 1.74420f, 2.07694f, 2.08560f, 1.88839f, 0.78191f, 0.93118f });

            names.Add("RShoulderRoll");
            times.Add(new[] { 1.30000f, 2.40000f, 2.90000f, 3.40000f, 3.90000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { -0.56762f, -0.93026f, -0.76271f, -0.55327f, -0.52534f, -0.55327f, -1.55683f, -0.54921f, -0.26389f });

            names.Add("RElbowYaw");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 3.90000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { 0.13963f, 0.00456f, -0.06907f, -0.14731f, 0.11694f, 1.75179f, 1.12745f, 0.51845f });

            names.Add("RElbowRoll");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 3.90000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { 0.83776f, 0.47865f, 0.07828f, 0.10129f, 0.59167f, 1.03856f, 1.29320f, 1.28247f });

            names.Add("RWristYaw");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 3.90000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { 0.18864f, 1.05535f, 1.05228f, 1.04308f, 1.04308f, 1.04768f, 1.06149f, -0.05987f });

            names.Add("RHand");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 3.90000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { 0.00709f, 0.00149f, 0.00154f, 0.00180f, 0.00206f, 0.00201f, 0.00211f, 0.00523f });

            names.Add("LHipYawPitch");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 4.30000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { -0.39726f, -0.42334f, -0.90809f, -0.58595f, -0.53993f, -0.79304f, -0.77770f, -0.61356f });

            names.Add("LHipRoll");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 4.30000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { 0.18259f, 0.31416f, -0.26227f, 0.29304f, 0.52927f, 0.23321f, 0.35746f, 0.27923f });

            names.Add("LHipPitch");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 4.30000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { -0.59055f, -0.86974f, -0.00916f, -0.62123f, -1.55390f, -1.59378f, -1.46493f, -1.57998f });

            names.Add("LKneePitch");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 4.30000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { 2.11253f, 2.11253f, 1.81774f, 1.98189f, 1.07687f, 1.15813f, 1.08756f, 1.39743f });

            names.Add("LAnklePitch");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 4.30000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { -1.18952f, -1.18952f, -1.18952f, -0.52314f, 0.73935f, 0.92275f, 0.92189f, 0.83599f });

            names.Add("LAnkleRoll");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 4.30000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { 0.04299f, 0.07981f, 0.04146f, -0.45402f, -0.18097f, -0.04751f, -0.05058f, -0.00916f });

            names.Add("RHipRoll");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 4.30000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { -0.13802f, -0.25614f, -0.38039f, -0.57521f, -0.53532f, -0.21472f, -0.32210f, -0.26841f });

            names.Add("RHipPitch");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 4.30000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { -0.76551f, -0.85908f, -1.36530f, -1.65063f, -1.54171f, -1.60921f, -1.41746f, -1.58620f });

            names.Add("RKneePitch");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 4.30000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { 1.89300f, 0.77471f, 1.14287f, 1.14594f, 1.03847f, 1.19810f, 0.98027f, 1.40672f });

            names.Add("RAnklePitch");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 4.30000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { -0.73014f, 0.33445f, 0.45717f, 0.77164f, 0.71335f, 0.93206f, 0.93206f, 0.84988f });

            names.Add("RAnkleRoll");
            times.Add(new[] { 1.30000f, 2.40000f, 3.40000f, 4.30000f, 5.20000f, 5.90000f, 6.60000f, 7.20000f });
            keys.Add(new[] { 0.21020f, 0.50626f, 0.05833f, -0.05518f, 0.18872f, 0.03532f, 0.04760f, 0.01845f });

            ChangePosition(names, times, keys);
        }

        private void ChangePositionFromBackToSitDown()
        {
            var bodyPart = NAOBodyParts.Arms.ToString();
            motionProxy.setCollisionProtectionEnabled(bodyPart, false);

            List<string> names = new List<string>();
            List<float[]> times = new List<float[]>();
            List<float[]> keys = new List<float[]>();

            names.Add("HeadYaw");
            times.Add(new[] { 1.00000f, 2.00000f, 2.80000f, 3.60000f, 4.40000f, 5.00000f, 5.60000f, 6.20000f });
            keys.Add(new[] { -0.02459f, -0.00000f, 0.04138f, 0.00000f, -0.00618f, 0.01376f, 0.02450f, -0.02305f });

            names.Add("HeadPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 2.80000f, 3.60000f, 4.40000f, 5.00000f, 5.60000f, 6.20000f });
            keys.Add(new[] { 0.06285f, 0.00000f, 0.51490f, -0.67200f, -0.02152f, 0.19478f, 0.07359f, -0.03532f });

            names.Add("LShoulderPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 2.60000f, 3.60000f, 4.40000f, 5.00000f, 5.60000f, 5.90000f, 6.20000f });
            keys.Add(new[] { 2.08560f, 2.09440f, 2.03865f, 2.08560f, 2.08567f, 1.30846f, 0.90348f, 0.70337f, 0.90195f });

            names.Add("LShoulderRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 2.60000f, 3.60000f, 4.40000f, 5.00000f, 5.60000f, 6.20000f });
            keys.Add(new[] { 0.98018f, 0.36652f, 0.34907f, 0.34907f, 0.40143f, 1.17193f, 0.39726f, 0.21625f });

            names.Add("LElbowYaw");
            times.Add(new[] { 1.00000f, 2.00000f, 2.60000f, 3.60000f, 4.40000f, 5.00000f, 5.60000f, 6.20000f });
            keys.Add(new[] { -0.76091f, 0.15708f, 0.12268f, 0.08727f, 0.05978f, -0.95419f, -0.96339f, -0.43877f });

            names.Add("LElbowRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 2.60000f, 3.60000f, 4.40000f, 4.60000f, 5.00000f, 5.60000f, 6.20000f });
            keys.Add(new[] { -0.62430f, 0.00000f, -1.54462f, -1.56210f, -0.87441f, -1.03498f, -0.67952f, -1.12285f, -1.23483f });

            names.Add("LWristYaw");
            times.Add(new[] { 1.00000f, 2.60000f, 4.40000f, 5.00000f, 6.20000f });
            keys.Add(new[] { -0.70568f, -1.78868f, -0.71949f, -0.04760f, 0.05978f });

            names.Add("LHand");
            times.Add(new[] { 1.00000f, 2.60000f, 4.40000f, 5.00000f, 6.20000f });
            keys.Add(new[] { 0.00088f, 0.00133f, 0.00158f, 0.00022f, 0.00524f });

            names.Add("RShoulderPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 2.60000f, 3.60000f, 4.40000f, 5.00000f, 5.60000f, 5.90000f, 6.20000f });
            keys.Add(new[] { 2.08560f, 2.09440f, 1.95283f, 2.08560f, 2.08567f, 1.91908f, 1.03703f, 0.72955f, 0.92504f });

            names.Add("RShoulderRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 2.60000f, 3.60000f, 4.40000f, 5.00000f, 5.60000f, 6.20000f });
            keys.Add(new[] { -1.07384f, -0.36652f, -0.34907f, -0.34907f, -0.41015f, -0.89897f, -0.74096f, -0.24241f });

            names.Add("RElbowYaw");
            times.Add(new[] { 1.00000f, 2.00000f, 2.60000f, 3.60000f, 4.40000f, 5.00000f, 5.60000f, 6.20000f });
            keys.Add(new[] { 0.76542f, -0.15708f, -0.25775f, -0.08727f, -0.06600f, 1.57691f, 1.06762f, 0.52459f });

            names.Add("RElbowRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 2.60000f, 3.60000f, 4.40000f, 4.60000f, 5.00000f, 5.60000f, 6.20000f });
            keys.Add(new[] { 0.71028f, 0.00000f, 1.54462f, 1.56210f, 0.71384f, 1.05418f, 0.95726f, 1.14594f, 1.27019f });

            names.Add("RWristYaw");
            times.Add(new[] { 1.00000f, 2.60000f, 4.40000f, 5.00000f, 6.20000f });
            keys.Add(new[] { 1.15199f, 1.72571f, 1.11057f, 0.24693f, -0.05680f });

            names.Add("RHand");
            times.Add(new[] { 1.00000f, 2.00000f, 2.60000f, 4.40000f, 5.00000f, 6.20000f });
            keys.Add(new[] { 0.00000f, 0.00000f, 0.00032f, 0.00005f, 0.00221f, 0.00523f });

            names.Add("LHipYawPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.10000f, 3.60000f, 4.40000f, 5.00000f, 6.20000f });
            keys.Add(new[] { -0.47090f, 0.00000f, 0.00771f, -0.00000f, -0.65498f, -0.70407f, -0.61816f });

            names.Add("LHipRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.10000f, 3.60000f, 4.40000f, 5.00000f, 6.20000f });
            keys.Add(new[] { 0.04453f, 0.00000f, 0.05833f, 0.00000f, -0.06439f, 0.14083f, 0.27923f });

            names.Add("LHipPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.10000f, 3.60000f, 4.40000f, 5.00000f, 6.20000f });
            keys.Add(new[] { 0.25008f, -0.17453f, 0.37434f, -0.17453f, -1.17807f, -1.57080f, -1.57998f });

            names.Add("LKneePitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.10000f, 3.60000f, 4.40000f, 5.00000f, 6.20000f });
            keys.Add(new[] { 0.09200f, 1.67552f, 2.02024f, 1.67552f, 1.07683f, 1.40161f, 1.39590f });

            names.Add("LAnklePitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.10000f, 3.60000f, 4.40000f, 5.00000f, 6.20000f });
            keys.Add(new[] { 0.82525f, 0.24435f, -0.45411f, 0.24435f, 0.67799f, 0.77609f, 0.83599f });

            names.Add("LAnkleRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.10000f, 3.60000f, 4.40000f, 5.00000f, 6.20000f });
            keys.Add(new[] { -0.03371f, 0.00000f, -0.03371f, 0.00000f, 0.17032f, 0.09083f, -0.00916f });

            names.Add("RHipRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.10000f, 3.60000f, 4.40000f, 5.00000f, 6.20000f });
            keys.Add(new[] { -0.14876f, 0.00000f, -0.02297f, 0.00000f, 0.06140f, -0.12200f, -0.26841f });

            names.Add("RHipPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.10000f, 3.60000f, 4.40000f, 5.00000f, 6.20000f });
            keys.Add(new[] { 0.18557f, -0.17453f, 0.28221f, -0.17453f, -1.16435f, -1.57080f, -1.59233f });

            names.Add("RKneePitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.10000f, 3.60000f, 4.40000f, 5.00000f, 6.20000f });
            keys.Add(new[] { 0.19179f, 1.67552f, 2.02645f, 1.67552f, 1.09532f, 1.37682f, 1.40825f });

            names.Add("RAnklePitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.10000f, 3.60000f, 4.40000f, 5.00000f, 6.20000f });
            keys.Add(new[] { 0.46638f, 0.24435f, -0.37425f, 0.24435f, 0.71795f, 0.76985f, 0.84988f });

            names.Add("RAnkleRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.10000f, 3.60000f, 4.40000f, 5.00000f, 6.20000f });
            keys.Add(new[] { 0.16725f, 0.00000f, -0.03984f, 0.00000f, -0.14415f, -0.11992f, 0.01845f });



            ChangePosition(names, times, keys);
            motionProxy.setCollisionProtectionEnabled(bodyPart, true);
        }

        private void ChangePositionFromBellyToCrouch()
        {
            List<string> names = new List<string>();
            List<float[]> times = new List<float[]>();
            List<float[]> keys = new List<float[]>();

            names.Add("HeadYaw");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.17453f, -0.22689f, 0.28623f, 0.29671f, -0.49567f, -0.29671f, 0.05236f, -0.39095f, -0.01745f });

            names.Add("HeadPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { -0.57683f, -0.54768f, 0.10734f, 0.51487f, 0.38048f, 0.37119f, -0.10472f, -0.53387f, 0.00698f });

            names.Add("LShoulderPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.08433f, -1.51146f, -1.25025f, 0.07206f, 1.27409f, 0.75573f, 0.75049f, 1.29154f, 1.51708f });

            names.Add("LShoulderRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 1.55390f, 0.01683f, 0.07666f, 0.07052f, 0.15643f, 0.93899f, 0.67719f, 0.84648f, 0.08433f });

            names.Add("LElbowYaw");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { -2.07694f, -1.58006f, -1.60461f, -1.78715f, -1.32695f, -1.24791f, -0.97260f, -0.95993f, -0.90203f });

            names.Add("LElbowRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { -0.00873f, -0.35278f, -0.63810f, -0.85133f, -1.55083f, -0.73304f, -0.73653f, -1.15506f, -1.09984f });

            names.Add("LWristYaw");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f });
            keys.Add(new[] { -1.55092f, -1.55092f, -1.51717f, -1.44814f, -1.40519f, -1.40825f, -1.40825f, -0.54454f });

            names.Add("LHand");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f });
            keys.Add(new[] { 0.00102f, 0.00034f, 0.00071f, 0.00095f, 0.00100f, 0.00100f, 0.00096f, 0.00100f });

            names.Add("RShoulderPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { -0.02757f, -1.51146f, -1.22256f, -0.23619f, 0.21787f, 0.44950f, 0.91431f, 0.96033f, 1.35303f });

            names.Add("RShoulderRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { -1.53558f, -0.19199f, -0.08288f, -0.08288f, -0.22707f, -0.18259f, -0.00870f, -0.13197f, -0.01385f });

            names.Add("RElbowYaw");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 2.07694f, 1.56157f, 1.61373f, 1.68582f, 1.96041f, 1.95121f, 0.66571f, 0.39573f, 0.86667f });

            names.Add("RElbowRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.10472f, 0.38201f, 0.63512f, 1.55705f, 0.00870f, 0.00870f, 0.42343f, 0.64926f, 0.83454f });

            names.Add("RWristYaw");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f });
            keys.Add(new[] { 1.50941f, 1.50941f, 1.49714f, 1.42811f, 1.43578f, 1.44038f, 1.44345f, 1.44038f });

            names.Add("RHand");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f });
            keys.Add(new[] { 0.00032f, 0.00022f, 0.00046f, 0.00082f, 0.00086f, 0.00084f, 0.00083f, 0.00084f });

            names.Add("LHipYawPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { -0.03371f, 0.03491f, -0.43561f, -0.77923f, -1.04154f, -1.14530f, -1.14530f, -0.56754f, 0.00000f });

            names.Add("LHipRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.06294f, 0.00004f, 0.00158f, -0.37732f, -0.29755f, -0.29755f, 0.19486f, 0.12736f, -0.01047f });

            names.Add("LHipPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.06140f, 0.00004f, -1.56924f, -1.28085f, -1.03694f, -1.15966f, -1.18267f, -1.27011f, -1.04720f });

            names.Add("LKneePitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.12043f, 1.98968f, 2.11253f, 0.28221f, 0.40493f, 0.35738f, 0.71940f, 2.01409f, 2.09440f });

            names.Add("LAnklePitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.92189f, -1.02974f, -1.15054f, 0.21625f, 0.71020f, 0.92275f, 0.82525f, -0.50166f, -1.04720f });

            names.Add("LAnkleRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { -0.00149f, 0.00004f, -0.00149f, -0.45249f, -0.30062f, -0.11808f, -0.04138f, -0.12114f, 0.00000f });

            names.Add("RHipRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.03142f, 0.00004f, 0.00158f, 0.31144f, 0.25469f, 0.32065f, 0.22707f, -0.07512f, 0.01047f });

            names.Add("RHipPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.07666f, -0.00004f, -1.57699f, -1.66136f, -1.19963f, -1.59847f, -0.32218f, -0.71028f, -1.04720f });

            names.Add("RKneePitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { -0.07819f, 1.98968f, 2.08628f, 1.74267f, 2.12019f, 2.12019f, 2.12019f, 2.12019f, 2.09440f });

            names.Add("RAnklePitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.92965f, -1.02974f, -1.13819f, -1.18645f, -1.18645f, -0.58901f, -1.18645f, -1.18645f, -1.04720f });

            names.Add("RAnkleRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.18850f, 0.00004f, 0.00618f, -0.00456f, -0.09813f, -0.01376f, -0.09507f, 0.03532f, 0.00000f });


            ChangePosition(names, times, keys);
        }

        private void ChangePositionFromSideToBelly()
        {
            List<string> names = new List<string>();
            List<float[]> times = new List<float[]>();
            List<float[]> keys = new List<float[]>();

            names.Add("HeadYaw");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { 0.72094f, 0.72094f });

            names.Add("HeadPitch");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { 0.14876f, 0.14876f });

            names.Add("LShoulderPitch");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { 1.89752f, 1.89752f });

            names.Add("LShoulderRoll");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { 0.80224f, 0.80224f });

            names.Add("LElbowYaw");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { -0.99254f, -0.99254f });

            names.Add("LElbowRoll");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { -0.46783f, -0.46783f });

            names.Add("LWristYaw");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { -0.67807f, -0.67807f });

            names.Add("LHand");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { 0.00350f, 0.00350f });

            names.Add("RShoulderPitch");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { 1.97891f, 1.97891f });

            names.Add("RShoulderRoll");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { -0.77931f, -0.77931f });

            names.Add("RElbowYaw");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { 0.92343f, 0.92343f });

            names.Add("RElbowRoll");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { 0.34059f, 0.34059f });

            names.Add("RWristYaw");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { 0.64577f, 0.64577f });

            names.Add("RHand");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { 0.00256f, 0.00256f });

            names.Add("LHipYawPitch");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { -1.14432f, -1.14432f });

            names.Add("LHipRoll");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { 0.25162f, 0.25162f });

            names.Add("LHipPitch");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { 0.03532f, 0.03532f });

            names.Add("LKneePitch");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { -0.09208f, -0.09208f });

            names.Add("LAnklePitch");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { 0.35585f, 0.35585f });

            names.Add("LAnkleRoll");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { 0.34366f, 0.34366f });

            names.Add("RHipRoll");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { -0.03064f, -0.03064f });

            names.Add("RHipPitch");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { 0.41567f, 0.41567f });

            names.Add("RKneePitch");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { -0.09200f, -0.09200f });

            names.Add("RAnklePitch");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { 0.52314f, 0.52314f });

            names.Add("RAnkleRoll");
            times.Add(new[] { 0.50000f, 1.00000f });
            keys.Add(new[] { -0.06745f, -0.06745f });


            ChangePosition(names, times, keys);

        }

        private NAOPositions GetPosition()
        {
            var curPositionAndTime = (System.Collections.ArrayList)robotPoseProxy.getActualPoseAndTime();
            string curPositionStr = curPositionAndTime[0].ToString();
            NAOPositions curPosition;
            Enum.TryParse<NAOPositions>(curPositionStr, true, out curPosition);
            return curPosition;
        }

        private void ChangePosition(List<string> names, List<float[]> times, List<float[]> keys)
        {
            motionProxy.angleInterpolation(names, keys, times, true);
        }      
    }
}
