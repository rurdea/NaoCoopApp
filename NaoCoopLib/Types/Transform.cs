using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaoCoopLib.Types
{
    public class Transform
    {
        /** \cond PRIVATE */
        public float r1_c1, r1_c2, r1_c3, r1_c4;
        public float r2_c1, r2_c2, r2_c3, r2_c4;
        public float r3_c1, r3_c2, r3_c3, r3_c4;
        /** \endcond */

        public Transform()
        {
            r1_c1 = 1.0f;
            r1_c2 = 0.0f;
            r1_c3 = 0.0f;
            r1_c4 = 0.0f;
            r2_c1 = 0.0f;
            r2_c2 = 1.0f;
            r2_c3 = 0.0f;
            r2_c4 = 0.0f;
            r3_c1 = 0.0f;
            r3_c2 = 0.0f;
            r3_c3 = 1.0f;
            r3_c4 = 0.0f;
        }

        public Transform(List<float> pFloats)
        {
            if (
              (pFloats.Count == 12) ||
              (pFloats.Count == 16))
            {
                r1_c1 = pFloats[0];
                r1_c2 = pFloats[1];
                r1_c3 = pFloats[2];
                r1_c4 = pFloats[3];

                r2_c1 = pFloats[4];
                r2_c2 = pFloats[5];
                r2_c3 = pFloats[6];
                r2_c4 = pFloats[7];

                r3_c1 = pFloats[8];
                r3_c2 = pFloats[9];
                r3_c3 = pFloats[10];
                r3_c4 = pFloats[11];

                /*
              // todo: check it is a real transform
              if (!isTransform())
              {
                normalizeTransform();

                // strange case with nan data
                if (!isTransform())
                {
                  std::cerr << "ALMath: WARNING: "
                            << "Transform constructor with wrong vector value. "
                            << "Rotation part is normalized." << std::endl;
                }
              }
                */
            }
            else
            {
                /*
              std::cerr << "ALMath: WARNING: "
                        << "Transform constructor call with a wrong size of vector. "
                        << "Size expected: 12 or 16. Size given: " << pFloats.size() << ". "
                        << "Transform is set to identity." << std::endl;
                */
                r1_c1 = 1.0f;
                r1_c2 = 0.0f;
                r1_c3 = 0.0f;
                r1_c4 = 0.0f;

                r2_c1 = 0.0f;
                r2_c2 = 1.0f;
                r2_c3 = 0.0f;
                r2_c4 = 0.0f;

                r3_c1 = 0.0f;
                r3_c2 = 0.0f;
                r3_c3 = 1.0f;
                r3_c4 = 0.0f;
            }
        }

        public Transform(float pPosX, float pPosY, float pPosZ)
        {
            r1_c1 = 1.0f;
            r1_c2 = 0.0f;
            r1_c3 = 0.0f;

            r2_c1 = 0.0f;
            r2_c2 = 1.0f;
            r2_c3 = 0.0f;

            r3_c1 = 0.0f;
            r3_c2 = 0.0f;
            r3_c3 = 1.0f;

            r1_c4 = pPosX;
            r2_c4 = pPosY;
            r3_c4 = pPosZ;
        }


        public static Transform operator *(Transform t1, Transform t2)
        {
            Transform t = new Transform();
            t.r1_c1 = (t1.r1_c1 * t2.r1_c1) + (t1.r1_c2 * t2.r2_c1) + (t1.r1_c3 * t2.r3_c1);
            t.r1_c2 = (t1.r1_c1 * t2.r1_c2) + (t1.r1_c2 * t2.r2_c2) + (t1.r1_c3 * t2.r3_c2);
            t.r1_c3 = (t1.r1_c1 * t2.r1_c3) + (t1.r1_c2 * t2.r2_c3) + (t1.r1_c3 * t2.r3_c3);
            t.r1_c4 = (t1.r1_c1 * t2.r1_c4) + (t1.r1_c2 * t2.r2_c4) + (t1.r1_c3 * t2.r3_c4) + t1.r1_c4;

            t.r2_c1 = (t1.r2_c1 * t2.r1_c1) + (t1.r2_c2 * t2.r2_c1) + (t1.r2_c3 * t2.r3_c1);
            t.r2_c2 = (t1.r2_c1 * t2.r1_c2) + (t1.r2_c2 * t2.r2_c2) + (t1.r2_c3 * t2.r3_c2);
            t.r2_c3 = (t1.r2_c1 * t2.r1_c3) + (t1.r2_c2 * t2.r2_c3) + (t1.r2_c3 * t2.r3_c3);
            t.r2_c4 = (t1.r2_c1 * t2.r1_c4) + (t1.r2_c2 * t2.r2_c4) + (t1.r2_c3 * t2.r3_c4) + t1.r2_c4;

            t.r3_c1 = (t1.r3_c1 * t2.r1_c1) + (t1.r3_c2 * t2.r2_c1) + (t1.r3_c3 * t2.r3_c1);
            t.r3_c2 = (t1.r3_c1 * t2.r1_c2) + (t1.r3_c2 * t2.r2_c2) + (t1.r3_c3 * t2.r3_c2);
            t.r3_c3 = (t1.r3_c1 * t2.r1_c3) + (t1.r3_c2 * t2.r2_c3) + (t1.r3_c3 * t2.r3_c3);
            t.r3_c4 = (t1.r3_c1 * t2.r1_c4) + (t1.r3_c2 * t2.r2_c4) + (t1.r3_c3 * t2.r3_c4) + t1.r3_c4;
            return t;
        }

        public float Determinant()
        {
            return this.r1_c1 * this.r2_c2 * this.r3_c3 +
                this.r1_c2 * this.r2_c3 * this.r3_c1 +
                this.r1_c3 * this.r2_c1 * this.r3_c2 -
                this.r1_c1 * this.r2_c3 * this.r3_c2 -
                this.r1_c2 * this.r2_c1 * this.r3_c3 -
                this.r1_c3 * this.r2_c2 * this.r3_c1;
        }

        /*
private void NormalizeTransform()
    {
      const float lEpsilon = 0.0001f;

      // z.normalize();
      float lNorm =
          std::sqrt(std::pow(pT.r1_c3, 2) +
                    std::pow(pT.r2_c3, 2) +
                    std::pow(pT.r3_c3, 2));
      if (lNorm < lEpsilon)
      {
        std::cerr << "ALMath: WARNING: "
                  << "normalizeTransform with null column. "
                  << "Rotation part set to identity." << std::endl;

        pT.r1_c1 = 1.0f;
        pT.r1_c2 = 0.0f;
        pT.r1_c3 = 0.0f;

        pT.r2_c1 = 0.0f;
        pT.r2_c2 = 1.0f;
        pT.r2_c3 = 0.0f;

        pT.r3_c1 = 0.0f;
        pT.r3_c2 = 0.0f;
        pT.r3_c3 = 1.0f;
        return;
      }

      pT.r1_c3 /= lNorm;
      pT.r2_c3 /= lNorm;
      pT.r3_c3 /= lNorm;

      // x = cross(y, z);
      const float x1 = pT.r2_c2*pT.r3_c3 - pT.r3_c2*pT.r2_c3;
      const float x2 = pT.r3_c2*pT.r1_c3 - pT.r1_c2*pT.r3_c3;
      const float x3 = pT.r1_c2*pT.r2_c3 - pT.r2_c2*pT.r1_c3;

      // x.normalize();
      lNorm = std::sqrt(std::pow(x1, 2) +
                        std::pow(x2, 2) +
                        std::pow(x3, 2));

      if (lNorm < lEpsilon)
      {
        std::cerr << "ALMath: WARNING: "
                  << "normalizeTransform with null column. "
                  << "Rotation part set to identity." << std::endl;

        pT.r1_c1 = 1.0f;
        pT.r1_c2 = 0.0f;
        pT.r1_c3 = 0.0f;

        pT.r2_c1 = 0.0f;
        pT.r2_c2 = 1.0f;
        pT.r2_c3 = 0.0f;

        pT.r3_c1 = 0.0f;
        pT.r3_c2 = 0.0f;
        pT.r3_c3 = 1.0f;
        return;
      }

      pT.r1_c1 = x1/lNorm;
      pT.r2_c1 = x2/lNorm;
      pT.r3_c1 = x3/lNorm;

      // y = cross(z, x);
      pT.r1_c2 = pT.r2_c3*pT.r3_c1 - pT.r3_c3*pT.r2_c1;
      pT.r2_c2 = pT.r3_c3*pT.r1_c1 - pT.r1_c3*pT.r3_c1;
      pT.r3_c2 = pT.r1_c3*pT.r2_c1 - pT.r2_c3*pT.r1_c1;
    }
         * */


    }
}
