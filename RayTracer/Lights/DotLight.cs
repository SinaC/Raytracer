using System;
using MathUtils;

namespace RayTracer.Lights
{
    public class DotLight : Light
    {
        public Vector3 Position { get; set; }
        public Color Color { get; set; }

        public DotLight(Vector3 position, Color color)
        {
            Position = position;
            Color = color;
        }

        public override Color GetColor(Intersection intersection, Scene scene)
        {
            Vector3 direction = Position - intersection.IntersectionPoint;
            direction.Normalize();
            Vector3 position = intersection.IntersectionPoint + direction*0.001;

            // TODO: shadow
            //Ray lightRay = new Ray(position, direction);
            //bool shadow = scene.Intersects(lightRay) != null;
            //if (shadow)
            //    return new Color(0, 0, 0);
            
            // Diffuse
            double diffuse = 0;
            if (intersection.SceneObject.Texture.Finish.Diffuse > 0)
            {
                double illuminationAngle = Vector3.DotProduct(direction, intersection.NormalAtIntersection);
                if (illuminationAngle > 0)
                    diffuse = illuminationAngle*intersection.SceneObject.Texture.Finish.Diffuse;
            }

            // Specular
            double specular = 0;
            if (intersection.SceneObject.Texture.Finish.Specular > 0)
            {
                Vector3 h = direction - intersection.Ray.Direction;
                h.Normalize();
                double specularAngle = Vector3.DotProduct(h, intersection.NormalAtIntersection);
                if (specularAngle > 0)
                    specular = intersection.SceneObject.Texture.Finish.Specular * Math.Pow(specularAngle, intersection.SceneObject.Texture.Finish.SpecularRoughness);
            }

            // Phong
            double phong = 0;
            if (intersection.SceneObject.Texture.Finish.Phong > 0)
            {
                double phongAngle = Vector3.DotProduct(direction, intersection.ReflectionAtIntersection);
                if (phongAngle > 0)
                    phong = intersection.SceneObject.Texture.Finish.Phong * Math.Pow(phongAngle, intersection.SceneObject.Texture.Finish.PhongSize);
            }

            // Combine diffuse, specular, phong // TODO: soft shading
            return Color * ((diffuse + specular + phong)); // TODO: multiply by brillance ?
        }
    }
}
