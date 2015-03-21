using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopLib.Types
{
    public class NaoPosition
    {
        public float x { get; set; }

       public float y { get; set; }

       public float z { get; set; }

        public NaoPosition()
        {

        }

        public NaoPosition(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public NaoPosition(List<float> pFloats)
        {
            if (pFloats != null && pFloats.Count == 3)
            {
                x = pFloats[0];
                y = pFloats[1];
                z = pFloats[2];
            }
            else
            {
                x = 0.0f;
                y = 0.0f;
                z = 0.0f;
            }
        }

        public List<float> ToFloats()
        {
            return new List<float> { x, y, z };
        }

       public static NaoPosition Pose2DInverse(NaoPosition pPos)
       {
           pPos.z = -pPos.z;

           float cos = (float)Math.Cos(pPos.z);
           float sin = (float)Math.Sin(pPos.z);
           float x = pPos.x;

           pPos.x = -(x * cos - pPos.y * sin);
           pPos.y = -(pPos.y * cos + x * sin);

           return pPos;
       }

       public static NaoPosition operator *(NaoPosition p1, NaoPosition p2)
       {
           var x = p1.x + Math.Cos(p1.z) * p2.x - Math.Sin(p1.z) * p2.y;
           var y = p1.y + Math.Sin(p1.z) * p2.x + Math.Cos(p1.z) * p2.y;
           var theta = p1.z + p2.z;

           return new NaoPosition() { x = (float)x, z = theta, y = (float)y };
       }
    }
}
