using System;

namespace MathUtils
{
    public static class EquationSolver
    {
        private const float Epsilon = 0.000000001f;

        public static int SolveQuadric(float[] x, float[] results)
        {
            // x[0] * y^2 + x[1] * y + x[2]
            //  a            b          c
            // discr = b^2 - 4*a*c
            float discr = x[1]*x[1] - 4.0f*x[0]*x[2];
            if (discr < 0.0)
                return 0;
            int index = 0;
            float sqrt = (float)Math.Sqrt(discr);
            results[index++] = (-x[1] - sqrt)/(2.0f*x[0]);
            results[index++] = (-x[1] + sqrt)/(2.0f*x[0]);
            return index;
        }

        public static int SolveCubic(float[] x, float[] results)
        {
            float a1, a2, a3;

            float a0 = x[0];
            if (Math.Abs(a0 - 0.0) < Epsilon)
                return 0;
            else
            {
                if (Math.Abs(a0 - 1.0) > Epsilon)
                {
                    a1 = x[1]/a0;
                    a2 = x[2]/a0;
                    a3 = x[3]/a0;
                }
                else
                {
                    a1 = x[1];
                    a2 = x[2];
                    a3 = x[3];
                }
            }

            float sQ;
            float a12 = a1*a1;
            float q = (a12 - 3.0f*a2)/9.0f;
            float r = (a1*(a12 - 4.5f*a2) + 13.5f*a3)/27.0f;
            float q3 = q*q*q;
            float r2 = r*r;
            float d = q3 - r2;
            float an = a1/3.0f;

            if (d >= 0.0)
            {
                // Three real roots.
                d = r/(float)Math.Sqrt(q3);
                float theta = (float)Math.Acos(d)/3.0f;
                sQ = -2.0f*(float)Math.Sqrt(q);

                results[0] = sQ*(float)Math.Cos(theta) - an;
                results[1] = sQ* (float)Math.Cos(theta + 2*Math.PI/3) - an;
                results[2] = sQ* (float)Math.Cos(theta + 4*Math.PI/3) - an;
                return 3;
            }
            else
            {
                sQ = (float)Math.Pow(Math.Sqrt(r2 - q3) + Math.Abs(r), 1.0/3.0);
                if (r < 0)
                    results[0] = (sQ + q/sQ) - an;
                else
                    results[0] = -(sQ + q/sQ) - an;
                return 1;
            }
        }

        public static int SolveQuartic(float[] x, float[] results)
        {
            float c1, c2, c3, c4;

            // Make sure the quartic has a leading coefficient of 1.0
            float c0 = x[0];
            if (Math.Abs(c0 - 1.0) > Epsilon)
            {
                c1 = x[1]/c0;
                c2 = x[2]/c0;
                c3 = x[3]/c0;
                c4 = x[4]/c0;
            }
            else
            {
                c1 = x[1];
                c2 = x[2];
                c3 = x[3];
                c4 = x[4];
            }

            // Compute the cubic resolvant
            float c12 = c1*c1;
            float p = -0.37500000f*c12 + c2;
            float q = 0.12500000f*c12*c1 - 0.5000f*c1*c2 + c3;
            float r = -0.01171875f*c12*c12 + 0.0625f*c12*c2 - 0.25f*c1*c3 + c4;

            float[] cubic = new float[4];
            float[] roots = new float[3];

            cubic[0] = 1.0f;
            cubic[1] = -0.5f*p;
            cubic[2] = -r;
            cubic[3] = 0.5f*r*p - 0.125f*q*q;

            int i = SolveCubic(cubic, roots);

            float z, d2;

            if (i > 0) 
                z = roots[0];
            else 
                return 0;

            float d1 = 2.0f*z - p;
            if (d1 < 0.0)
            {
                if (d1 > -Epsilon)
                    d1 = 0.0f;
                else
                    return 0;
            }

            if (d1 < Epsilon)
            {
                d2 = z*z - r;
                if (d2 < 0.0) 
                    return 0;
                d2 = (float)Math.Sqrt(d2);
            }
            else
            {
                d1 = (float)Math.Sqrt(d1);
                d2 = 0.5f*q/d1;
            }

            // Set up useful values for the quadratic factors
            float q1 = d1*d1;
            float q2 = -0.25f*c1;
            i = 0;

            // Solve the first quadratic
            p = q1 - 4.0f*(z - d2);
            if (Math.Abs(p - 0.0) < Epsilon)
                results[i++] = -0.5f*d1 - q2;
            else
            {
                if (p > 0)
                {
                    p = (float)Math.Sqrt(p);
                    results[i++] = -0.5f*(d1 + p) + q2;
                    results[i++] = -0.5f*(d1 - p) + q2;
                }
            }

            // Solve the second quadratic
            p = q1 - 4.0f*(z + d2);
            if (Math.Abs(p - 0.0) < Epsilon)
                results[i++] = 0.5f*d1 - q2;
            else
            {
                if (p > 0)
                {
                    p = (float)Math.Sqrt(p);
                    results[i++] = 0.5f*(d1 + p) + q2;
                    results[i++] = 0.5f*(d1 - p) + q2;
                }
            }

            return i;
        }
    }
}
