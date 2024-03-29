using System;
using System.Numerics;

namespace RayTracer.Engine.Geometries;

public class Plane : GeometryBase
{
    public Vector3 Normal { get; private set; }
    public float Distance { get; private set; }

    public Plane(Vector3 normal, float distance)
    {
        Distance = distance;
        Normal = Vector3.Normalize(normal);
    }

    public override bool HasIntersections(Ray ray)
    {
        //double tmp = Vector3.DotProduct(ray.Direction, Normal);
        //return Math.Abs(tmp) >= Epsilon;
        return false; // quick and dirty hack, infinite planes have no interesting shadows
    }

    public override bool ComputeNearestIntersection(Ray ray, out float t)
    {
        t = float.MinValue;
        // plane -> Nx * x + Ny * y + Nz * z + D = 0
        // ray -> R(t) = Ro + Rd * t   Ro = (Xo,Yo,Zo)  Rd = (Xd,Yd,Zd)
        // ==> ray equation in plane equation ==>
        // Nx*(Xo+Xd*t) + Ny*(Yo+Yd*t) + Nz*(Zo+Zd*t) + D = 0
        // t = - (N dot Ro + D) + (N dot Rd)   if |N dot Rd| == 0 -> parallel (no intersection)
        float tmp = Vector3.Dot(ray.Direction, Normal);
        if (Math.Abs(tmp) < Epsilon) // is line parallel to plane ?
            return false;
        t = -(Vector3.Dot(ray.Origin, Normal) + Distance) / tmp;
        return true;
    }

    public override Vector3 ComputeNormal(Vector3 point)
        => Normal;

    public override bool Contains(Vector3 point)
        => false;
}
