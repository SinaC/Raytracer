using System;
using System.Numerics;

namespace RayTracer.Engine.Geometries;

// TODO: remove 'a' from computation
public class Sphere : GeometryBase
{
    public Vector3 Center { get; private set; }
    public float Radius { get; private set; }

    public Sphere(Vector3 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public override bool HasIntersections(Ray ray)
    {
        //float a = 1;
        //float b = 2*(Vector3.Dot(ray.Origin, ray.Direction) - Vector3.Dot(ray.Direction, Center));
        //float c = (ray.Origin - Center).LengthSquared() - Radius*Radius;
        //return b*b - 4*a*c >= 0;
        Vector3 diff = Center - ray.Origin;
        float t = Vector3.Dot(diff, ray.Direction);
        if (t < 0)
            return false;
        diff -= ray.Direction * t;
        float dist2 = diff.LengthSquared();
        return dist2 <= Radius * Radius;
    }

    public override bool ComputeNearestIntersection(Ray ray, out float t)
    {
        t = float.MinValue;
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
        // a == 1 and b is even -->
        // b2 = (Xo*Xd - Xd*Xc + Yo*Yd - Yd*Yc + Zo*Zd - Zd*Zc) = (o.d) - (d.c)
        // c = ||o-c||^2 - R^2
        // delta2 = b2^2-c
        // t0 = -b2 - sqrt(delta)
        // t1 = -b2 + sqrt(delta)
        float b = Vector3.Dot(ray.Origin, ray.Direction) - Vector3.Dot(ray.Direction, Center);
        float c = (ray.Origin - Center).LengthSquared() - Radius*Radius;
        float delta = b*b - c;
        if (delta < 0)
            return false;
        float sqrtDelta = (float)Math.Sqrt(delta);
        float t0 = (-b - sqrtDelta);
        if (t0 > 0)
        {
            t = t0;
            return true;
        }
        float t1 = (-b + sqrtDelta);
        t = t1;
        return true;
    }

    public override Vector3 ComputeNormal(Vector3 point)
    {
        Vector3 normal = point - Center;
        return Vector3.Normalize(normal);
    }

    public override bool Contains(Vector3 point)
    {
        // Add a little bit to the actual radius to be more tolerant
        // of rounding errors that would incorrectly exclude a 
        // point that should be inside the sphere.
        float r = Radius + Epsilon;

        // A point is inside the sphere if the square of its distance 
        // from the center is within the square of the radius.
        Vector3 diff = point - Center;
        return diff.LengthSquared() <= (r * r);
    }
}
