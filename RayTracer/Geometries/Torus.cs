using System;
using MathUtils;

namespace RayTracer.Geometries
{
    public class Torus : Geometry
    {
        public double MajorRadius { get; private set; }
        public double MinorRadius { get; private set; }

        public Torus(double majorRadius, double minorRadius)
        {
            MajorRadius = majorRadius;
            MinorRadius = minorRadius;
        }

        public override bool HasIntersections(Ray ray)
        {
            double t;
            return ComputeNearestIntersection(ray, out t);
        }

        public override bool ComputeNearestIntersection(Ray ray, out double t)
        {
            double[] c = new double[5];
            double[] r = new double[4];

            double major2 = MajorRadius*MajorRadius;
            double minor2 = MinorRadius*MinorRadius;

            double od = Vector3.DotProduct(ray.Origin, ray.Direction);
            double oo = Vector3.DotProduct(ray.Origin, ray.Origin);
            double k = oo - minor2 - major2;
            
            c[0] = 1.0;
            c[1] = 4*od;
            c[2] = 2*(2*od*od + k + 2*major2*ray.Direction.Z*ray.Direction.Z);
            c[3] = 4*(k*od + 2*major2*ray.Origin.Z*ray.Direction.Z);
            c[4] = k*k + 4*major2*(ray.Origin.Z*ray.Origin.Z - minor2);

            int n = EquationSolver.SolveQuartic(c, r);
            bool found = GetSmallestPositive(r, n, out t);

            return found;
        }

        public override Vector3 ComputeNormal(Vector3 point)
        {
            double a = 1 - (MajorRadius / Math.Sqrt(point.X * point.X + point.Y * point.Y));
            Vector3 normal = new Vector3(point.X * a, point.Y * a, point.Z);
            normal.Normalize();
            return normal;
        }

        public override bool Contains(Vector3 point)
        {
            double t = MajorRadius - Math.Sqrt(point.X*point.X + point.Y*point.Y);
            double f = t*t + point.Z*point.Z - MinorRadius*MinorRadius;
            return f <= Epsilon;
        }

        private static bool GetSmallestPositive(double[] v, int n, out double solution)
        {
            bool found = false;
            solution = double.PositiveInfinity;
            for (int i = 0; i < n; i++)
                if (v[i] > 0.0f && v[i] < solution)
                {
                    solution = v[i];
                    found = true;
                }
            return found;
        }
    }
}
