using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aldebaran.Proxies;

namespace NaoCoopLib.Helpers
{
    public class MotionHelper
    {
        public static void StiffnessOn(MotionProxy proxy)
        {
            // We use the "Body" name to signify the collection of all joints
            var pNames = "Body";
            var pStiffnessLists = 1.0f;
            var pTimeLists = 1.0f;
            proxy.stiffnessInterpolation(pNames, pStiffnessLists, pTimeLists);
        }

        public static void MoveHead(MotionProxy motionProxy, float? headPitch, float? headYaw, bool isAbsolute, float time = 1.5f)
        {
            if (motionProxy == null || (!headPitch.HasValue && !headYaw.HasValue))
            {
                return;
            }

            List<string> names = new List<string>();
            List<float[]> times = new List<float[]>();
            List<float[]> keys = new List<float[]>();

            if (headPitch.HasValue)
            {
                names.Add("HeadPitch");
                keys.Add(new[] { headPitch.Value });
                times.Add(new[] { time });
            }

            if (headYaw.HasValue)
            {
                names.Add("HeadYaw");
                keys.Add(new[] { headYaw.Value });
                times.Add(new[] { time });
            }

            var methodID = motionProxy.post.angleInterpolation(names, keys, times, isAbsolute);
            motionProxy.wait(methodID, 0);
            
        }

        public static float DegreeToRadian(float degrees)
        {
            return (float)(degrees * Math.PI / 180);
        }

        public static float RadianToDegree(float radians)
        {
            return (float)(180 * radians / Math.PI);
        }

    }
}
