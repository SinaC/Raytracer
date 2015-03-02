using System.Linq;
using MathUtils;

namespace RayTracer
{
    public class Tracer
    {
        public Scene Scene { get; private set; }
        public Camera Camera { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Tracer(Scene scene, Camera camera, int width, int height) // TODO: screen
        {
            Scene = scene;
            Camera = camera;
            Width = width;
            Height = height;
        }

        public Color[,] Render()
        {
            Color[,] bitmap = new Color[Width,Height];
            for (int y = 0; y < Height; y++)
                for(int x = 0; x < Width; x++)
                {
                    Vector3 direction = Camera.ComputeRayDirection(Width, Height, x, y);
                    Ray ray = new Ray(Camera.Eye, direction);
                    Color color = TraceRay(ray, 0);
                    bitmap[x, Height-y-1] = color;
                }
            return bitmap;
        }

        private Color TraceRay(Ray ray, int depth) // TODO: shader
        {
            Intersection intersection = Scene.NearestIntersection(ray);
            if (intersection == null)
                return new Color(0,0,0); // TODO: background shader
            //else
            //    System.Diagnostics.Debug.WriteLine("TraceRay: intersection T: {0:F6}", intersection.T);
            return Shade(intersection, depth);
        }

        private Color Shade(Intersection intersection, int depth) // TODO: shader
        {
            // TODO: ambient
            Color diffuseColor = GetDiffuseColor(intersection);
            // TODO: handle reflection and transmission/refraction
            return diffuseColor * intersection.SceneObject.Texture.Pigment.ComputeColor(intersection.IntersectionPoint);
        }

        private Color GetDiffuseColor(Intersection intersection)
        {
            Color color = new Color(0, 0, 0);
            return Scene.Lights
                .Select(light => light.GetColor(intersection, Scene))
                .Aggregate(color, (current, lightColor) => current + lightColor);
        }
    }
}
