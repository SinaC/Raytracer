using System;
using MathUtils;

namespace RayTracer.Geometries
{
    // TODO: remove 'a' from computation
    public class Sphere : Geometry
    {
        public Vector3 Center { get; private set; }
        public double Radius { get; private set; }

        public Sphere(Vector3 center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        public override bool HasIntersections(Ray ray)
        {
            double a = 1;
            double b = 2*(Vector3.DotProduct(ray.Origin, ray.Direction) - Vector3.DotProduct(ray.Direction, Center));
            double c = (ray.Origin - Center).LengthSquared - Radius*Radius;
            return b*b - 4*a*c >= 0;
        }

        public override bool ComputeNearestIntersection(Ray ray, out double t)
        {
            t = Double.MinValue;
            // sphere -> (X-Xc)^2 + (Y-Yc)^2 + (Z-Zc)^2 = R^2
            // ray -> R(t) = Ro + Rd * t   Ro = (Xo,Yo,Zo)  Rd = (Xd,Yd,Zd)
            // (Xo+Xd*t-Xc)^2+(Yo+Yd*t-Yc)^2+(Zo+Zd*t-Zc)^2 = R^2
            // a*t^2 + b*t + c = 0
            // a -> Xd^2+Yd^2+Zd^2 -> equals 1 because ray is normalized
            // b -> 2*(Xo*Xd - Xd*Xc + Yo*Yd - Yd*Yc + Zo*Zd - Zd*Zc)
            // c -> (Xo-Xc)^2 + (Yo-Yc)^2 + (Zo-Zc)^2 - R^2
            // delta = b^2 - 4*a*c
            // t0 = -b - sqrt(delta) / 2*a // if positive, it's the smaller
            // t1  = -b + sqrt(delta) / 2*a // else, it's the smaller
            double a = 1;
            double b = 2*(Vector3.DotProduct(ray.Origin, ray.Direction) - Vector3.DotProduct(ray.Direction, Center));
            double c = (ray.Origin - Center).LengthSquared - Radius*Radius;
            double delta = b*b - 4*a*c;
            if (delta < 0)
                return false;
            double sqrtDelta = Math.Sqrt(delta);
            double t0 = (-b - sqrtDelta)/(2*a);
            if (t0 > 0)
            {
                t = t0;
                return true;
            }
            double t1 = (-b + sqrtDelta)/(2*a);
            t = t1;
            return true;
        }

        public override Vector3 ComputeNormal(Vector3 point)
        {
            Vector3 normal = point - Center;
            normal.Normalize();
            return normal;
        }
    }
}
