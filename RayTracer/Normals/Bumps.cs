using MathUtils;
using RayTracer.Turbulences;

namespace RayTracer.Normals
{
    public class Bumps : Normal
    {
        public double Factor { get; private set; }

        public Bumps(double factor)
        {
            Factor = factor;
        }

        // Doesn't provide visible results :/
        public override Vector3 PerturbNormal(Intersection intersection)
        {
            float x = SimplexNoise.Generate((float)intersection.IntersectionPoint.X/20);
            float y = SimplexNoise.Generate((float)intersection.IntersectionPoint.Y/20);
            float z = SimplexNoise.Generate((float)intersection.IntersectionPoint.Z/20);
            return new Vector3(intersection.RawNormalAtIntersection.X + x * Factor, intersection.RawNormalAtIntersection.Y + y * Factor, intersection.RawNormalAtIntersection.Z + z * Factor);
        }
    }
}
