using RayTracer.Engine.Normals;
using RayTracer.Engine.Pigments;

namespace RayTracer.Engine;

public class Material
{
    public Finish Finish { get; init; }
    public PigmentBase Pigment { get; init; }
    public NormalBase Normal { get; init; }
    public Interior Interior { get; init; }

    public Material(Finish finish, PigmentBase pigment, NormalBase normal = null, Interior interior = null)
    {
        Finish = finish;
        Pigment = pigment;
        Normal = normal;
        Interior = interior;
    }
}
