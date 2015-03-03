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
            //float x = SimplexNoise.Generate((float)intersection.IntersectionPoint.X);
            //float y = SimplexNoise.Generate((float)intersection.IntersectionPoint.Y);
            //float z = SimplexNoise.Generate((float)intersection.IntersectionPoint.Z);
            //Vector3 newNormal = new Vector3(intersection.RawNormalAtIntersection.X + x * Factor, intersection.RawNormalAtIntersection.Y + y * Factor, intersection.RawNormalAtIntersection.Z + z * Factor);
            //newNormal.Normalize();
            //return newNormal;

            double noise = SimplexNoise.Generate((float)(intersection.IntersectionPoint.X * Factor), (float)(intersection.IntersectionPoint.Y * Factor), (float)(intersection.IntersectionPoint.Z * Factor));
            Vector3 newNormal = new Vector3(intersection.RawNormalAtIntersection.X + noise, intersection.RawNormalAtIntersection.Y + noise, intersection.RawNormalAtIntersection.Z + noise);
            newNormal.Normalize();
            return newNormal;
        }
    }
}
