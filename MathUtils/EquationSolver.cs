using System;

namespace MathUtils
{
    public static class EquationSolver
    {
        private const double Epsilon = 0.000000001;

        public static int SolveQuadric(double[] x, double[] results)
        {
            // x[0] * y^2 + x[1] * y + x[2]
            //  a            b          c
            // discr = b^2 - 4*a*c
            double discr = x[1]*x[1] - 4.0*x[0]*x[2];
            if (discr < 0.0)
                return 0;
            int index = 0;
            double sqrt = Math.Sqrt(discr);
            results[index++] = (-x[1] - sqrt)/(2.0*x[0]);
            results[index++] = (-x[1] + sqrt)/(2.0*x[0]);
            return index;
        }

        public static int SolveCubic(double[] x, double[] results)
        {
            double a1, a2, a3;

            double a0 = x[0];
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

            double sQ;
            double a12 = a1*a1;
            double q = (a12 - 3.0*a2)/9.0;
            double r = (a1*(a12 - 4.5*a2) + 13.5*a3)/27.0;
            double q3 = q*q*q;
            double r2 = r*r;
            double d = q3 - r2;
            double an = a1/3.0;

            if (d >= 0.0)
            {
                // Three real roots.
                d = r/Math.Sqrt(q3);
                double theta = Math.Acos(d)/3.0;
                sQ = -2.0*Math.Sqrt(q);

                results[0] = sQ*Math.Cos(theta) - an;
                results[1] = sQ*Math.Cos(theta + 2*Math.PI/3) - an;
                results[2] = sQ*Math.Cos(theta + 4*Math.PI/3) - an;
                return 3;
            }
            else
            {
                sQ = Math.Pow(Math.Sqrt(r2 - q3) + Math.Abs(r), 1.0/3.0);
                if (r < 0)
                    results[0] = (sQ + q/sQ) - an;
                else
                    results[0] = -(sQ + q/sQ) - an;
                return 1;
            }
        }

        public static int SolveQuartic(double[] x, double[] results)
        {
            double c1, c2, c3, c4;

            // Make sure the quartic has a leading coefficient of 1.0
            double c0 = x[0];
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
            double c12 = c1*c1;
            double p = -0.37500000*c12 + c2;
            double q = 0.12500000*c12*c1 - 0.5000*c1*c2 + c3;
            double r = -0.01171875*c12*c12 + 0.0625*c12*c2 - 0.25*c1*c3 + c4;

            double[] cubic = new double[4];
            double[] roots = new double[3];

            cubic[0] = 1.0;
            cubic[1] = -0.5*p;
            cubic[2] = -r;
            cubic[3] = 0.5*r*p - 0.125*q*q;

            int i = SolveCubic(cubic, roots);

            double z, d2;

            if (i > 0) 
                z = roots[0];
            else 
                return 0;

            double d1 = 2.0*z - p;
            if (d1 < 0.0)
            {
                if (d1 > -Epsilon)
                    d1 = 0.0;
                else
                    return 0;
            }

            if (d1 < Epsilon)
            {
                d2 = z*z - r;
                if (d2 < 0.0) 
                    return 0;
                d2 = Math.Sqrt(d2);
            }
            else
            {
                d1 = Math.Sqrt(d1);
                d2 = 0.5*q/d1;
            }

            // Set up useful values for the quadratic factors
            double q1 = d1*d1;
            double q2 = -0.25*c1;
            i = 0;

            // Solve the first quadratic
            p = q1 - 4.0*(z - d2);
            if (Math.Abs(p - 0.0) < Epsilon)
                results[i++] = -0.5*d1 - q2;
            else
            {
                if (p > 0)
                {
                    p = Math.Sqrt(p);
                    results[i++] = -0.5*(d1 + p) + q2;
                    results[i++] = -0.5*(d1 - p) + q2;
                }
            }

            // Solve the second quadratic
            p = q1 - 4.0*(z + d2);
            if (Math.Abs(p - 0.0) < Epsilon)
                results[i++] = 0.5*d1 - q2;
            else
            {
                if (p > 0)
                {
                    p = Math.Sqrt(p);
                    results[i++] = 0.5*(d1 + p) + q2;
                    results[i++] = 0.5*(d1 - p) + q2;
                }
            }

            return i;
        }
    }
}
