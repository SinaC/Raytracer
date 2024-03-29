using RayTracer.Engine.Turbulences;
using System.Numerics;

namespace RayTracer.Engine.Normals;

public class Bumps : NormalBase
{
    public float Factor { get; private set; }

    public Bumps(float factor)
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

        float noise = SimplexNoise.Generate(intersection.IntersectionPoint.X * Factor, intersection.IntersectionPoint.Y * Factor, intersection.IntersectionPoint.Z * Factor);
        Vector3 newNormal = new Vector3(intersection.RawNormalAtIntersection.X + noise, intersection.RawNormalAtIntersection.Y + noise, intersection.RawNormalAtIntersection.Z + noise);
        return Vector3.Normalize(newNormal);
    }
}
