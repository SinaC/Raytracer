using MathUtils;

namespace RayTracer.Pigments
{
    public class Solid : Pigment
    {
        public Color Color { get; private set; }

        public Solid(Color color)
        {
            Color = color;
        }

        public override Color ComputeColor(Vector3 point)
        {
            return Color;
        }
    }
}
