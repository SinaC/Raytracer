using MathUtils;

namespace RayTracer.Geometries
{
    // Line-Geometry intersection
    public abstract class Geometry
    {
        public const double Epsilon = 0.000001;

        public abstract bool HasIntersections(Ray ray);
        public abstract bool ComputeNearestIntersection(Ray ray, out double t);
        public abstract Vector3 ComputeNormal(Vector3 point);
        public abstract bool Contains(Vector3 point);
    }
}
