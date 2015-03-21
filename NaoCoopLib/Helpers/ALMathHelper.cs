using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NaoCoopLib.Types;

namespace NaoCoopLib.Helpers
{
    public class ALMathHelper
    {
        public static Transform TransformFrom3DRotation(float pWX, float pWY, float pWZ)
        {
            Transform T = TransformFromRotZ(pWZ);
            T = T * TransformFromRotY(pWY);
            T = T * TransformFromRotX(pWX);
            return T;
        }

        public static Transform TransformFromRotZ(float pRotZ)
        {
            float c = (float)Math.Cos(pRotZ);
            float s = (float)Math.Sin(pRotZ);
            Transform T = new Transform();
            T.r1_c1 = c;
            T.r1_c2 = -s;
            T.r2_c1 = s;
            T.r2_c2 = c;
            return T;
        }

        public static Transform TransformFromRotX(float pRotX)
        {
            float c = (float)Math.Cos(pRotX);
            float s = (float)Math.Sin(pRotX);
            Transform T = new Transform();
            T.r2_c2 = c;
            T.r2_c3 = -s;
            T.r3_c2 = s;
            T.r3_c3 = c;
            return T;
        }

        public static Transform TransformFromRotY(float pRotY)
        {
            float c = (float)Math.Cos(pRotY);
            float s = (float)Math.Sin(pRotY);
            Transform T = new Transform();
            T.r1_c1 = c;
            T.r1_c3 = s;
            T.r3_c1 = -s;
            T.r3_c3 = c;
            return T;
        }
    }
}
