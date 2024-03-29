using RayTracer.Engine.Cameras;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace RayTracer.Engine;

public class Tracer
{
    private const float SmallForwardStep = 0.01f;
    private const int MaxDepth = 6;
    private const float MinAttenuation = 0.03f;

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
        //Color[,] bitmap = new Color[Width, Height];
        //for (int y = 0; y < Height; y++)
        //    for (int x = 0; x < Width; x++)
        //    {
        //        Vector3 direction = Camera.ComputeRayDirection(Width, Height, x, y);
        //        Ray ray = new Ray(Camera.Eye, direction);
        //        Color color = TraceRay(ray, 0, 1.0f);
        //        bitmap[x, Height - y - 1] = color;
        //    }
        //return bitmap;

        Color[,] bitmap = new Color[Width, Height];
        Parallel.For(0, Height, y =>
            {
                for (int x = 0; x < Width; x++)
                {
                    Vector3 direction = Camera.ComputeRayDirection(Width, Height, x, y);
                    Ray ray = new Ray(Camera.Eye, direction);
                    Color color = TraceRay(ray, 0, 1.0f);
                    bitmap[x, Height - y - 1] = color;
                }
            });
        return bitmap;
    }

    public Color TraceRay(Ray ray, int depth, float attenuation) // TODO: shader
    {
        Intersection intersection = Scene.NearestIntersection(ray);
        if (intersection == null)
            return Color.Black; // TODO: background shader
        //else
        //    System.Diagnostics.Debug.WriteLine("TraceRay: intersection T: {0:F6}", intersection.T);
        return Shade(intersection, depth, attenuation);
    }

    private Color Shade(Intersection intersection, int depth, float attenuation) // TODO: shader
    {
        if (attenuation < MinAttenuation)
            return Color.Black;

        // TODO: ambient
        Color diffuseColor = ComputeDiffuseColor(intersection);
        Color reflectedColor = Color.Black;
        if (intersection.SceneObject.Material.Finish.Reflection > 0 && depth < MaxDepth)
            reflectedColor = ComputeReflectedColor(intersection, depth, attenuation);
        Color transmittedColor = Color.Black;
        // TODO: debug transmission
        if (intersection.SceneObject.Material.Finish.Transmission > 0 && intersection.SceneObject.Material.Interior != null && depth < MaxDepth)
            transmittedColor = ComputeTransmittedColor(intersection, depth, attenuation);

        return attenuation * (diffuseColor + reflectedColor + transmittedColor);
    }

    private Color ComputeDiffuseColor(Intersection intersection)
    {
        //Color color = new Color(0, 0, 0);
        //color = Scene.Lights
        //    .Select(light => light.GetColor(intersection, Scene))
        //    .Aggregate(color, (current, lightColor) => current + lightColor);
        //return intersection.SceneObject.Texture.Pigment.ComputeColor(intersection.IntersectionPoint)*color;
         Color color = Scene.Lights
            .Select(light => light.GetColor(intersection, Scene))
            .Aggregate(Color.Black, (current, lightColor) => current + lightColor);
        return intersection.SceneObject.Material.Pigment.ComputeColor(intersection.IntersectionPoint) * color;
    }

    private Color ComputeReflectedColor(Intersection intersection, int depth, float attenuation)
    {
        // Compute new ray from intersection point and reflected direction
        Vector3 origin = intersection.IntersectionPoint + (intersection.ReflectionAtIntersection * SmallForwardStep); // avoid colliding immediately with intersected object
        Ray ray = new Ray(origin, intersection.ReflectionAtIntersection);
        Color color = TraceRay(ray, depth + 1, attenuation*intersection.SceneObject.Material.Finish.Reflection);
        return color;
    }

    private Color ComputeTransmittedColor(Intersection intersection, int depth, float attenuation)
    {
        // TODO: determine if already within a object -> correct IOR


        // ray is entering from atmosphere
        float ratio = Interior.Air/intersection.SceneObject.Material.Interior.IndexOfRefraction;

        // Compute transmitted ray
        Vector3 normal = intersection.NormalAtIntersection;
        float ci = Vector3.Dot(intersection.Ray.Direction, normal);
        if (ci <= 0)
            ci = -ci;
        else
            normal = -normal;
        float ratio2 = ratio*ratio;
        float ci2 = ci*ci;
        float criticalAngle = 1.0f + ratio2*(ci2 - 1.0f);
        if (criticalAngle < 0) // total internal reflection
        {
            Vector3 direction = intersection.Ray.Direction - ((2 * ci) * normal);
            Vector3 origin = intersection.IntersectionPoint + (direction * SmallForwardStep); // avoid colliding immediately with intersected object
            Ray ray = new Ray(origin, direction);
            Color color = TraceRay(ray, depth + 1, attenuation * intersection.SceneObject.Material.Finish.Transmission) * (1 - intersection.SceneObject.Material.Finish.Reflection);
            return color;
        }
        else // normal transmission
        {
            Vector3 direction = (intersection.Ray.Direction * ratio) + (normal * (ratio * ci - (float)Math.Sqrt(criticalAngle)));
            Vector3 origin = intersection.IntersectionPoint + (direction * SmallForwardStep); // avoid colliding immediately with intersected object
            Ray ray = new Ray(origin, direction);
            Color color = TraceRay(ray, depth + 1, attenuation * intersection.SceneObject.Material.Finish.Transmission);
            return color;
        }
    }
}
