using MathUtils;

namespace RayTracer
{
    public class Ray
    {
        public Vector3 Origin { get; private set; }
        public Vector3 Direction { get; private set; } // Must be normalized

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }
    }
}
