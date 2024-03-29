using System;
using System.Numerics;

namespace RayTracer.Engine.Lights;

public class DotLight : LightBase
{
    public Vector3 Position { get; set; }
    public Color Color { get; set; }

    public DotLight(Vector3 position, Color color)
    {
        Position = position;
        Color = color;
    }

    public override bool IsLighten(Vector3 point)
    {
        return true;
    }

    public override Color GetColor(Intersection intersection, Scene scene)
    {
        Vector3 rawDirection = Position - intersection.IntersectionPoint;
        var direction = Vector3.Normalize(rawDirection);
        Vector3 position = intersection.IntersectionPoint + direction * 0.001f;

        // TODO: shadow
        Ray lightRay = new Ray(position, direction);
        bool shadow = scene.Intersects(lightRay) != null;
        if (shadow)
            return new Color(0, 0, 0);

        // Diffuse
        float diffuse = 0;
        if (intersection.SceneObject.Material.Finish.Diffuse > 0)
        {
            float illuminationAngle = Vector3.Dot(direction, intersection.NormalAtIntersection);
            if (illuminationAngle > 0)
                diffuse = illuminationAngle * intersection.SceneObject.Material.Finish.Diffuse;
        }

        // Specular
        float specular = 0;
        if (intersection.SceneObject.Material.Finish.Specular > 0)
        {
            Vector3 rawH = direction - intersection.Ray.Direction;
            var h = Vector3.Normalize(rawH);
            float specularAngle = Vector3.Dot(h, intersection.NormalAtIntersection);
            if (specularAngle > 0)
                specular = intersection.SceneObject.Material.Finish.Specular * (float)Math.Pow(specularAngle, intersection.SceneObject.Material.Finish.SpecularRoughness);
        }

        // Phong
        float phong = 0;
        if (intersection.SceneObject.Material.Finish.Phong > 0)
        {
            float phongAngle = Vector3.Dot(direction, intersection.ReflectionAtIntersection);
            //Vector3 lightReflection = intersection.NormalAtIntersection - (2 * Vector3.DotProduct(direction, intersection.NormalAtIntersection)) * direction;
            //float phongAngle = Vector3.DotProduct(lightReflection, intersection.NormalAtIntersection);
            if (phongAngle > 0)
                phong = intersection.SceneObject.Material.Finish.Phong * (float)Math.Pow(phongAngle, intersection.SceneObject.Material.Finish.PhongSize);
        }

        // Combine diffuse, specular, phong // TODO: soft shading
        return Color * ((diffuse + specular + phong)); // TODO: multiply by brillance ?
    }
}
