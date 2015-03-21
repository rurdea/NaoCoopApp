using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aldebaran.Proxies;
using NaoCoopLib.Enums;

namespace NaoCoopLib.Executers
{
    public class StandUpExecuter
    {
        MotionProxy motionProxy;
        RobotPoseProxy robotPoseProxy;

        public StandUpExecuter(MotionProxy motionProxy, RobotPoseProxy robotPoseProxy)
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

            motionProxy.stiffnessInterpolation("Body", 1, 1);
            while (curPosition != NAOPositions.Stand)
            {
                switch (curPosition)
                {
                    case NAOPositions.Sit:
                        ChangePositionFromSitToCrouch();
                        ChangePositionFromCrouchToStand();
                        break;
                    case NAOPositions.Crouch:
                        ChangePositionFromCrouchToStand();
                        break;
                    case NAOPositions.Belly:
                    case NAOPositions.Frog:
                    case NAOPositions.Knee:
                        ChangePositionFromBellyToStand();
                        break;
                    case NAOPositions.Back:
                        ChangePositionFromBackToStand();
                        break;
                    case NAOPositions.Right:
                    case NAOPositions.Left:
                        ChangePositionFromSideToBelly();
                        break;
                    case NAOPositions.HeadBack:
                        break;
                    case NAOPositions.Unknown:
                        return false;
                    default:
                        return false;
                }
                curPosition = GetPosition();
            }
            return true;
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

        private void ChangePositionFromBackToStand()
        {
            var bodyPart = NAOBodyParts.Arms.ToString();
            motionProxy.setCollisionProtectionEnabled(bodyPart, false);
            List<string> names = new List<string>();
            List<float[]> times = new List<float[]>();
            List<float[]> keys = new List<float[]>();

            names.Add("HeadYaw");
            times.Add(new[] { 1.00000f, 2.00000f, 2.80000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { -0.02459f, -0.00000f, 0.04138f, 0.00000f, 0.00000f, 0.00000f, -0.51393f, -0.30224f, 0.26354f, 0.12043f, -0.00940f, 0.06592f });

            names.Add("HeadPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 2.80000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { 0.06285f, 0.00000f, 0.51490f, -0.67200f, 0.00000f, 0.34907f, 0.17177f, 0.34511f, 0.01047f, 0.02967f, 0.37886f, -0.03993f });

            names.Add("LShoulderPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 2.60000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { 2.08560f, 2.08567f, 2.03865f, 2.08560f, 2.08560f, 2.08560f, 2.03251f, 1.28698f, 0.95717f, 0.73321f, 0.73321f, 1.59225f });

            names.Add("LShoulderRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 2.60000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.00000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { 0.98018f, 0.36652f, 0.34907f, 0.34907f, 0.36652f, 0.00870f, 0.29909f, 0.44331f, 0.07052f, 0.26687f, 0.49851f, 0.49851f, 0.21932f });

            names.Add("LElbowYaw");
            times.Add(new[] { 1.00000f, 2.00000f, 2.60000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { -0.76091f, 0.15708f, 0.12268f, 0.08727f, 0.08727f, 0.08727f, -1.96049f, -0.85448f, -0.25008f, 0.08308f, 0.08308f, -1.03089f });

            names.Add("LElbowRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 2.60000f, 3.60000f, 4.30000f, 4.80000f, 5.20000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { -0.62430f, -0.03491f, -1.54462f, -1.56210f, -0.69813f, -0.00870f, -0.19199f, -1.04615f, -0.87280f, -0.82985f, -0.80535f, -0.80535f, -0.68105f });

            names.Add("LWristYaw");
            times.Add(new[] { 1.00000f, 2.60000f, 5.60000f, 6.50000f, 7.40000f, 10.50000f });
            keys.Add(new[] { -0.70568f, -1.78868f, -0.72562f, -0.71949f, -0.72256f, -0.71182f });

            names.Add("LHand");
            times.Add(new[] { 1.00000f, 2.60000f, 5.60000f, 6.50000f, 7.40000f, 10.50000f });
            keys.Add(new[] { 0.00088f, 0.00133f, 0.00219f, 0.00269f, 0.00290f, 0.00451f });

            names.Add("RShoulderPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 2.60000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { 2.08560f, 2.08567f, 1.95283f, 2.08560f, 2.08560f, 2.08560f, 2.07708f, 2.03720f, 1.44047f, 0.89131f, 0.89131f, 1.46961f });

            names.Add("RShoulderRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 2.60000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { -1.07384f, -0.36652f, -0.34907f, -0.34907f, -0.36652f, -0.00870f, -0.45717f, -0.56455f, -0.70568f, -0.87266f, -0.68068f, -0.15037f });

            names.Add("RElbowYaw");
            times.Add(new[] { 1.00000f, 2.00000f, 2.60000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { 0.76542f, -0.15708f, -0.25775f, -0.08727f, -0.08727f, -0.08727f, -0.08134f, -0.06907f, -0.02612f, 0.00062f, 0.00062f, 0.91269f });

            names.Add("RElbowRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 2.60000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { 0.71028f, 0.03491f, 1.54462f, 1.56210f, 0.69813f, 0.00870f, 0.09668f, 0.02152f, 0.19026f, 0.45379f, 0.55995f, 0.38047f });

            names.Add("RWristYaw");
            times.Add(new[] { 1.00000f, 2.60000f, 5.60000f, 6.50000f, 7.40000f, 10.50000f });
            keys.Add(new[] { 1.15199f, 1.72571f, 1.07989f, 1.07836f, 1.07989f, 1.08450f });

            names.Add("RHand");
            times.Add(new[] { 1.00000f, 2.00000f, 2.60000f, 4.30000f, 5.60000f, 6.50000f, 7.40000f, 10.50000f });
            keys.Add(new[] { 0.00000f, 0.00000f, 0.00032f, 0.00000f, 0.00287f, 0.00300f, 0.00308f, 0.00376f });

            names.Add("LHipYawPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.20000f, 3.60000f, 4.30000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { -0.47090f, 0.00000f, 0.00771f, -0.00000f, -0.65498f, -0.49851f, -0.85900f, -0.69639f, -0.40225f, -0.40225f, -0.23006f });

            names.Add("LHipRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.20000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { 0.04453f, 0.00000f, 0.05833f, 0.00000f, -0.06478f, 0.54105f, 0.15498f, -0.17483f, 0.00925f, 0.19199f, 0.17453f, 0.15191f });

            names.Add("LHipPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.20000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { 0.25008f, -0.17453f, 0.37434f, -0.17453f, -1.17808f, -1.57080f, -0.85746f, -0.05672f, -0.45556f, -0.85521f, -0.83599f, 0.21327f });

            names.Add("LKneePitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.20000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { 0.09200f, 1.67552f, 2.02024f, 1.67552f, 1.07777f, 1.67552f, 2.11075f, 2.11227f, 2.11253f, 2.11253f, 2.11253f, -0.08901f });

            names.Add("LAnklePitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.20000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { 0.82525f, 0.24435f, -0.45411f, 0.24435f, 0.67748f, 0.66323f, -0.45564f, -1.10145f, -1.18952f, -1.18952f, -1.18952f, 0.08126f });

            names.Add("LAnkleRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.20000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { -0.03371f, 0.00000f, -0.03371f, 0.00000f, 0.17109f, -0.10472f, -0.39573f, -0.09967f, 0.04299f, 0.11810f, 0.08727f, -0.13648f });

            names.Add("RHipRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.20000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { -0.14876f, 0.00000f, -0.02297f, 0.00000f, 0.06190f, -0.54105f, -0.55833f, -0.61049f, -0.62657f, -0.29671f, -0.01745f, -0.06285f });

            names.Add("RHipPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.20000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { 0.18557f, -0.17453f, 0.28221f, -0.17453f, -1.16456f, -1.57080f, -1.52484f, -1.56012f, -1.02974f, -0.90583f, -0.90583f, 0.20858f });

            names.Add("RKneePitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.20000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { 0.19179f, 1.67552f, 2.02645f, 1.67552f, 1.09386f, 1.67552f, 1.22111f, 1.09992f, 1.08385f, 0.87616f, 1.76278f, -0.07666f });

            names.Add("RAnklePitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.20000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { 0.46638f, 0.24435f, -0.37425f, 0.24435f, 0.71737f, 0.66323f, 0.78545f, 0.78392f, 0.44157f, 0.40317f, -0.57945f, 0.08134f });

            names.Add("RAnkleRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.20000f, 3.60000f, 4.30000f, 4.80000f, 5.60000f, 6.50000f, 7.40000f, 8.00000f, 8.60000f, 10.50000f });
            keys.Add(new[] { 0.16725f, 0.00000f, -0.03984f, 0.00000f, -0.16084f, 0.10472f, 0.00925f, -0.09813f, 0.44331f, 0.67960f, 0.27751f, 0.06294f });


            ChangePosition(names, times, keys);
            motionProxy.setCollisionProtectionEnabled(bodyPart, true);
        }

        private void ChangePositionFromBellyToStand()
        {
            List<string> names = new List<string>();
            List<float[]> times = new List<float[]>();
            List<float[]> keys = new List<float[]>();

            names.Add("HeadYaw");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.00000f, -0.22689f, 0.28623f, 0.29671f, -0.49567f, -0.29671f, 0.05236f, -0.39095f, 0.06745f });

            names.Add("HeadPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { -0.67195f, -0.54768f, 0.10734f, 0.51487f, 0.38048f, 0.37119f, -0.10472f, -0.53387f, 0.12741f });

            names.Add("LShoulderPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.08433f, -1.51146f, -1.25025f, 0.07206f, 1.27409f, 0.75573f, 0.75049f, 1.29154f, 1.59378f });

            names.Add("LShoulderRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 1.55390f, 0.01683f, 0.07666f, 0.07052f, 0.15643f, 0.93899f, 0.67719f, 0.84648f, 0.21779f });

            names.Add("LElbowYaw");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { -2.07694f, -1.58006f, -1.60461f, -1.78715f, -1.32695f, -1.24791f, -0.97260f, -0.95993f, -1.01862f });

            names.Add("LElbowRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { -0.00873f, -0.35278f, -0.63810f, -0.85133f, -1.55083f, -0.73304f, -0.73653f, -1.15506f, -0.68105f });

            names.Add("LWristYaw");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { -1.55092f, -1.55092f, -1.51717f, -1.44814f, -1.40519f, -1.40825f, -1.40825f, -0.54454f, -0.71028f });

            names.Add("LHand");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.00102f, 0.00034f, 0.00071f, 0.00095f, 0.00100f, 0.00100f, 0.00096f, 0.00100f, 0.00455f });

            names.Add("RShoulderPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { -0.02757f, -1.51146f, -1.22256f, -0.23619f, 0.21787f, 0.44950f, 0.91431f, 0.96033f, 1.47882f });

            names.Add("RShoulderRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { -1.53558f, -0.19199f, -0.08288f, -0.08288f, -0.22707f, -0.18259f, -0.00870f, -0.13197f, -0.14884f });

            names.Add("RElbowYaw");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 2.07694f, 1.56157f, 1.61373f, 1.68582f, 1.96041f, 1.95121f, 0.66571f, 0.39573f, 0.90962f });

            names.Add("RElbowRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.10472f, 0.38201f, 0.63512f, 1.55705f, 0.00870f, 0.00870f, 0.42343f, 0.64926f, 0.39275f });

            names.Add("RWristYaw");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 1.50941f, 1.50941f, 1.49714f, 1.42811f, 1.43578f, 1.44038f, 1.44345f, 1.44038f, 1.08603f });

            names.Add("RHand");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.00032f, 0.00022f, 0.00046f, 0.00082f, 0.00086f, 0.00084f, 0.00083f, 0.00084f, 0.00380f });

            names.Add("LHipYawPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.02269f, 0.03491f, -0.43561f, -0.77923f, -1.04154f, -1.14530f, -1.14530f, -0.56754f, -0.23619f });

            names.Add("LHipRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.00000f, 0.00004f, 0.00158f, -0.37732f, -0.29755f, -0.29755f, 0.19486f, 0.12736f, 0.14884f });

            names.Add("LHipPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { -0.80460f, 0.00004f, -1.56924f, -1.28085f, -1.03694f, -1.15966f, -1.18267f, -1.27011f, 0.21327f });

            names.Add("LKneePitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { -0.09233f, 1.98968f, 2.11253f, 0.28221f, 0.40493f, 0.35738f, 0.71940f, 2.01409f, -0.09055f });

            names.Add("LAnklePitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.64403f, -1.02974f, -1.15054f, 0.21625f, 0.71020f, 0.92275f, 0.82525f, -0.50166f, 0.07666f });

            names.Add("LAnkleRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.00000f, 0.00004f, -0.00149f, -0.45249f, -0.30062f, -0.11808f, -0.04138f, -0.12114f, -0.13495f });

            names.Add("RHipRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.00000f, 0.00004f, 0.00158f, 0.31144f, 0.25469f, 0.32065f, 0.22707f, -0.07512f, -0.05978f });

            names.Add("RHipPitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { -0.80460f, -0.00004f, -1.57699f, -1.66136f, -1.19963f, -1.59847f, -0.32218f, -0.71028f, 0.21012f });

            names.Add("RKneePitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { -0.09233f, 1.98968f, 2.08628f, 1.74267f, 2.12019f, 2.12019f, 2.12019f, 2.12019f, -0.07666f });

            names.Add("RAnklePitch");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.64403f, -1.02974f, -1.13819f, -1.18645f, -1.18645f, -0.58901f, -1.18645f, -1.18645f, 0.08595f });

            names.Add("RAnkleRoll");
            times.Add(new[] { 1.00000f, 2.00000f, 3.40000f, 4.50000f, 5.80000f, 6.50000f, 7.60000f, 8.90000f, 10.00000f });
            keys.Add(new[] { 0.00000f, 0.00004f, 0.00618f, -0.00456f, -0.09813f, -0.01376f, -0.09507f, 0.03532f, 0.06140f });


            ChangePosition(names, times, keys);
        }

        private void ChangePositionFromCrouchToStand()
        {
            List<string> names = new List<string>();
            List<float[]> times = new List<float[]>();
            List<float[]> keys = new List<float[]>();


            names.Add("HeadYaw");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { 0.06745f });

            names.Add("HeadPitch");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { -0.04146f });

            names.Add("LShoulderPitch");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { 1.58611f });

            names.Add("LShoulderRoll");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { 0.22392f });

            names.Add("LElbowYaw");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { -1.02169f });

            names.Add("LElbowRoll");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { -0.65958f });

            names.Add("LWristYaw");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { -0.75170f });

            names.Add("LHand");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { 0.00456f });

            names.Add("RShoulderPitch");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { 1.46808f });

            names.Add("RShoulderRoll");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { -0.14731f });

            names.Add("RElbowYaw");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { 0.90962f });

            names.Add("RElbowRoll");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { 0.37127f });

            names.Add("RWristYaw");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { 1.05535f });

            names.Add("RHand");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { 0.00381f });

            names.Add("LHipYawPitch");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { -0.23926f });

            names.Add("LHipRoll");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { 0.14884f });

            names.Add("LHipPitch");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { 0.21173f });

            names.Add("LKneePitch");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { -0.09055f });

            names.Add("LAnklePitch");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { 0.08126f });

            names.Add("LAnkleRoll");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { -0.13342f });

            names.Add("RHipRoll");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { -0.05825f });

            names.Add("RHipPitch");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { 0.21165f });

            names.Add("RKneePitch");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { -0.07666f });

            names.Add("RAnklePitch");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { 0.07981f });

            names.Add("RAnkleRoll");
            times.Add(new[] { 1.50000f });
            keys.Add(new[] { 0.05987f });

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

        private void ChangePositionFromSitToCrouch()
        {
            List<string> names = new List<string>();
            List<float[]> times = new List<float[]>();
            List<float[]> keys = new List<float[]>();

            names.Add("HeadYaw");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 2.20000f, 3.00000f, 3.76000f, 5.04000f, 7.20000f });
            keys.Add(new[] { 0.00149f, -0.27925f, -0.34826f, -0.47124f, -0.52360f, -0.28690f, -0.00940f, -0.00940f });

            names.Add("HeadPitch");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 2.20000f, 3.00000f, 3.76000f, 5.04000f, 7.20000f });
            keys.Add(new[] { 0.37119f, 0.36965f, 0.22689f, 0.27576f, 0.15708f, 0.34971f, 0.37886f, 0.37886f });

            names.Add("LShoulderPitch");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 2.20000f, 2.56000f, 3.00000f, 3.76000f, 4.36000f, 5.04000f, 7.20000f });
            keys.Add(new[] { 0.49567f, 0.57421f, 0.62657f, 1.01393f, 0.65101f, 0.75162f, 1.12050f, 1.17286f, 1.40848f, 1.06814f });

            names.Add("LShoulderRoll");
            times.Add(new[] { 0.60000f, 0.84000f, 1.12000f, 1.56000f, 2.20000f, 2.56000f, 3.00000f, 3.76000f, 4.36000f, 5.04000f, 7.20000f });
            keys.Add(new[] { 0.24386f, 0.49393f, 0.41567f, 0.50964f, 0.75398f, 0.61087f, 0.51078f, 0.07666f, 0.15489f, 0.68872f, 0.59208f });

            names.Add("LElbowYaw");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 2.20000f, 2.56000f, 3.00000f, 3.76000f, 4.36000f, 5.04000f, 7.20000f });
            keys.Add(new[] { -0.54461f, -0.71182f, -0.73636f, -0.81766f, -0.29764f, -0.16265f, -0.86982f, -1.01402f, -0.92044f, -0.36513f });

            names.Add("LElbowRoll");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 2.20000f, 2.56000f, 3.00000f, 3.76000f, 4.36000f, 5.04000f, 7.20000f });
            keys.Add(new[] { -1.06302f, -0.93416f, -0.79304f, -0.94183f, -1.06762f, -0.62430f, -0.89121f, -0.85133f, -1.23636f, -1.07836f });

            names.Add("LWristYaw");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f });
            keys.Add(new[] { -0.74250f, -0.77164f, -0.73636f });

            names.Add("LHand");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f });
            keys.Add(new[] { 0.00032f, 0.00116f, 0.00126f });

            names.Add("RShoulderPitch");
            times.Add(new[] { 0.60000f, 0.84000f, 1.12000f, 1.56000f, 2.20000f, 3.00000f, 3.40000f, 3.76000f, 4.36000f, 5.04000f, 7.20000f });
            keys.Add(new[] { 0.56609f, 0.73304f, 1.32695f, 1.96664f, 2.08560f, 2.06787f, 2.08560f, 2.02799f, 1.61381f, 1.42359f, 1.21650f });

            names.Add("RShoulderRoll");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 1.92000f, 2.20000f, 3.00000f, 3.40000f, 3.76000f, 4.36000f, 5.04000f, 7.20000f });
            keys.Add(new[] { -0.35133f, -0.67500f, -0.63819f, -0.62832f, -0.00873f, -0.50933f, -0.55382f, -0.56455f, -0.66580f, -0.57683f, -0.26236f });

            names.Add("RElbowYaw");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 2.20000f, 3.00000f, 3.40000f, 3.76000f, 4.36000f, 5.04000f, 7.20000f });
            keys.Add(new[] { 0.48010f, 1.82235f, 2.05399f, -0.08727f, -0.08134f, -0.07674f, -0.06600f, 0.31596f, 1.67969f, 0.96638f });

            names.Add("RElbowRoll");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 1.92000f, 2.20000f, 3.00000f, 3.40000f, 3.76000f, 4.36000f, 5.04000f, 7.20000f });
            keys.Add(new[] { 1.21650f, 1.02936f, 0.68114f, 0.85521f, 0.00873f, 0.07981f, 0.05987f, 0.01692f, 0.33292f, 0.49706f, 0.97260f });

            names.Add("RWristYaw");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f });
            keys.Add(new[] { 1.07529f, 1.09523f, 1.08756f });

            names.Add("RHand");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f });
            keys.Add(new[] { 0.00032f, 0.00221f, 0.00244f });

            names.Add("LHipYawPitch");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 3.00000f, 3.28000f, 3.76000f, 5.04000f, 5.80000f, 7.20000f });
            keys.Add(new[] { -0.67645f, -0.65173f, -0.75144f, -0.49909f, -0.64559f, -0.85897f, -0.43408f, -0.28221f, -0.17330f });

            names.Add("LHipRoll");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 2.20000f, 3.00000f, 3.28000f, 3.76000f, 5.04000f, 5.80000f, 7.20000f });
            keys.Add(new[] { 0.13964f, 0.14501f, 0.14808f, 0.54105f, 0.15498f, -0.02526f, -0.17453f, 0.18259f, 0.44030f, 0.13043f });

            names.Add("LHipPitch");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 2.20000f, 3.00000f, 3.28000f, 3.76000f, 4.36000f, 5.04000f, 5.80000f, 7.20000f });
            keys.Add(new[] { -1.57077f, -1.57080f, -1.57080f, -1.57080f, -0.85706f, -0.59586f, -0.05236f, -0.40143f, -0.69026f, -0.75469f, -0.73014f });

            names.Add("LKneePitch");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 2.20000f, 3.00000f, 3.28000f, 3.76000f, 5.04000f, 5.80000f, 7.20000f });
            keys.Add(new[] { 0.72401f, 0.95923f, 1.18473f, 1.67552f, 2.11255f, 2.11255f, 2.11255f, 2.00182f, 2.11253f, 1.61833f });

            names.Add("LAnklePitch");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 2.20000f, 3.00000f, 3.28000f, 3.76000f, 5.04000f, 5.80000f, 7.20000f });
            keys.Add(new[] { 0.77616f, 0.75542f, 0.75542f, 0.66323f, -0.45379f, -0.68068f, -1.09956f, -1.18944f, -1.18944f, -0.73636f });

            names.Add("LAnkleRoll");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 2.20000f, 3.00000f, 3.28000f, 3.76000f, 5.04000f, 5.80000f, 7.20000f });
            keys.Add(new[] { -0.05058f, -0.00550f, 0.02671f, -0.17453f, -0.39573f, -0.08681f, -0.10379f, 0.01078f, -0.01683f, -0.08893f });

            names.Add("RHipRoll");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 2.20000f, 3.00000f, 3.28000f, 3.76000f, 5.04000f, 5.80000f, 7.20000f });
            keys.Add(new[] { -0.14415f, -0.13336f, -0.21006f, -0.54105f, -0.55842f, -0.73468f, -0.61087f, -0.25460f, -0.08126f, -0.07512f });

            names.Add("RHipPitch");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 2.20000f, 3.00000f, 3.28000f, 3.76000f, 5.04000f, 5.80000f, 7.20000f });
            keys.Add(new[] { -1.55245f, -1.57080f, -1.57080f, -1.57080f, -1.52484f, -1.57080f, -1.55965f, -0.84988f, -0.67807f, -0.64892f });

            names.Add("RKneePitch");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 2.20000f, 3.00000f, 3.28000f, 3.76000f, 5.04000f, 5.80000f, 7.20000f });
            keys.Add(new[] { 0.84374f, 0.95426f, 1.21504f, 1.67552f, 1.22173f, 1.27333f, 1.09956f, 0.59523f, 1.44047f, 1.58006f });

            names.Add("RAnklePitch");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 2.20000f, 3.00000f, 3.28000f, 3.76000f, 4.36000f, 4.72000f, 5.04000f, 5.80000f, 7.20000f });
            keys.Add(new[] { 0.78545f, 0.76027f, 0.76027f, 0.66323f, 0.78540f, 0.66976f, 0.78540f, 0.78540f, 0.54105f, 0.48172f, -0.36965f, -0.78843f });

            names.Add("RAnkleRoll");
            times.Add(new[] { 0.60000f, 1.12000f, 1.56000f, 2.20000f, 3.00000f, 3.28000f, 3.52000f, 3.76000f, 4.36000f, 4.72000f, 5.04000f, 5.80000f, 7.20000f });
            keys.Add(new[] { 0.04913f, 0.00073f, -0.00081f, 0.17453f, 0.00929f, 0.07436f, -0.15708f, -0.10472f, 0.24435f, 0.34907f, 0.44030f, 0.42343f, 0.09055f });

            ChangePosition(names, times, keys);

        }

        private void ChangePosition(List<string> names, List<float[]> times, List<float[]> keys)
        {
            motionProxy.angleInterpolation(names, keys, times, true);
        }
    }
}
