using RayTracer.Geometries;

namespace RayTracer
{
    public class SceneObject
    {
        public Geometry Geometry { get; private set; }
        public Material Material { get; private set; }

        public SceneObject(Geometry geometry, Material material)
        {
            Geometry = geometry;
            Material = material;
        }
    }
}
