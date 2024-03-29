using RayTracer.Engine.Turbulences;
using System.Numerics;

namespace RayTracer.Engine.Pigments;

public class Noisy : PigmentBase
{
    public float Factor { get; private set; }

    public Noisy(float factor)
    {
        Factor = factor;
    }

    public override Color ComputeColor(Vector3 point)
    {
        float noise = SimplexNoise.Generate(point.X * Factor, point.Y * Factor, (point.Z * Factor) + 1) * 0.5f;
        return new Color(noise, noise, noise);
    }
}
