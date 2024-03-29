using System.Numerics;

namespace RayTracer.Engine.Normals;

public abstract class NormalBase
{
    public abstract Vector3 PerturbNormal(Intersection intersection);
}
