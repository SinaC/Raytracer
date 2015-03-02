using MathUtils;

namespace RayTracer.Pigments
{
    public abstract class Pigment
    {
        public abstract Color ComputeColor(Vector3 point);
    }
}
