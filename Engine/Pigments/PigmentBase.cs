using System.Numerics;

namespace RayTracer.Engine.Pigments;

public abstract class PigmentBase
{
    public abstract Color ComputeColor(Vector3 point);
}
