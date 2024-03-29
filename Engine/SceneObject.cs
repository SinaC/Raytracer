using RayTracer.Engine.Geometries;

namespace RayTracer.Engine;

public class SceneObject
{
    public GeometryBase Geometry { get; private set; }
    public Material Material { get; private set; }

    public SceneObject(GeometryBase geometry, Material material)
    {
        Geometry = geometry;
        Material = material;
    }
}
