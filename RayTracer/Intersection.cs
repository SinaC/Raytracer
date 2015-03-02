using System;
using MathUtils;

namespace RayTracer
{
    public class Intersection
    {
        public static readonly Intersection NullObject = new Intersection(null, null, Double.MinValue);

        public SceneObject SceneObject { get; private set; }
        public Ray Ray { get; private set; }
        public double T { get; private set; }

        private Vector3 _intersectionPoint;
        private Vector3 _normalAtIntersection;
        private Vector3 _reflectionAtIntersection;

        public Intersection(SceneObject sceneObject, Ray ray, double t)
        {
            SceneObject = sceneObject;
            Ray = ray;
            T = t;
        }

        public Vector3 IntersectionPoint
        {
            get
            {
                _intersectionPoint = _intersectionPoint ?? Ray.Origin + Ray.Direction*T;
                return _intersectionPoint;
            }
        }

        public Vector3 NormalAtIntersection
        {
            get
            {
                _normalAtIntersection = _normalAtIntersection ?? SceneObject.Geometry.ComputeNormal(IntersectionPoint);
                return _normalAtIntersection;
            }
        }

        public Vector3 ReflectionAtIntersection
        {
            get
            {
                _reflectionAtIntersection = _reflectionAtIntersection ?? (Ray.Direction - (2 * Vector3.DotProduct(NormalAtIntersection, Ray.Direction)) * NormalAtIntersection);
                return _reflectionAtIntersection;
            }
        }
    }
}
