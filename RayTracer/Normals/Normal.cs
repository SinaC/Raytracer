using MathUtils;

namespace RayTracer.Normals
{
    public abstract class Normal
    {
        public abstract Vector3 PerturbNormal(Intersection intersection);
    }
}
