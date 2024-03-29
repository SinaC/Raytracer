using System.Numerics;

namespace RayTracer.Engine.Pigments;

public class Solid : PigmentBase
{
    public Color Color { get; private set; }

    public Solid(Color color)
    {
        Color = color;
    }

    public override Color ComputeColor(Vector3 point)
        => Color;
}
