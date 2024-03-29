using System.Numerics;

namespace RayTracer.Engine.Lights;

public abstract class LightBase
{
    public abstract bool IsLighten(Vector3 point);
    public abstract Color GetColor(Intersection intersection, Scene scene);
}
