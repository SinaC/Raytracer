using RayTracer.Geometries;

namespace RayTracer
{
    public class SceneObject
    {
        public Geometry Geometry { get; private set; }
        public Texture Texture { get; private set; }

        public SceneObject(Geometry geometry, Texture texture)
        {
            Geometry = geometry;
            Texture = texture;
        }
    }
}
