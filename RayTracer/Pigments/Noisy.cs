using MathUtils;
using RayTracer.Turbulences;

namespace RayTracer.Pigments
{
    public class Noisy : Pigment
    {
        public double Factor { get; private set; }

        public Noisy(double factor)
        {
            Factor = factor;
        }

        public override Color ComputeColor(Vector3 point)
        {
            double noise = (SimplexNoise.Generate((float)(point.X * Factor), (float)(point.Y * Factor), (float)(point.Z * Factor)) + 1) * 0.5;
            return new Color(noise, noise, noise);
        }
    }
}
