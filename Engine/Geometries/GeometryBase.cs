using System.Numerics;

namespace RayTracer.Engine.Geometries;

// Line-Geometry intersection
public abstract class GeometryBase
{
    public const float Epsilon = 0.000001f;

    public abstract bool HasIntersections(Ray ray);
    public abstract bool ComputeNearestIntersection(Ray ray, out float t);
    public abstract Vector3 ComputeNormal(Vector3 point);
    public abstract bool Contains(Vector3 point);
}
