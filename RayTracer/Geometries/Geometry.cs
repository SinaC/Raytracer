using MathUtils;

namespace RayTracer.Geometries
{
    // Line-Geometry intersection
    //http://www.realtimerendering.com/intersections.html
    //http://plib.sourceforge.net/sg/index.html

    public abstract class Geometry
    {
        public const double Epsilon = 0.000001;

        public abstract bool HasIntersections(Ray ray);
        public abstract bool ComputeNearestIntersection(Ray ray, out double t);
        public abstract Vector3 ComputeNormal(Vector3 point);
    }
}
