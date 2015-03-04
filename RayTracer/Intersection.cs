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

        private Vector3 _intersectionPoint; // cache computed value
        private Vector3 _normalAtIntersection; // cache computed value
        private Vector3 _reflectionAtIntersection; // cache computed value
        private Vector3 _perturbedNormalAtIntersection; // cache computed value

        public Intersection(SceneObject sceneObject, Ray ray, double t)
        {
            SceneObject = sceneObject;
            Ray = ray;
            T = t;
        }

        // Intersection point computed from ray and scene object
        public Vector3 IntersectionPoint
        {
            get
            {
                _intersectionPoint = _intersectionPoint ?? Ray.Origin + Ray.Direction*T;
                return _intersectionPoint;
            }
        }

        // Normal (non-perturbed) at intersection point
        public Vector3 RawNormalAtIntersection
        {
            get
            {
                _normalAtIntersection = _normalAtIntersection ?? SceneObject.Geometry.ComputeNormal(IntersectionPoint);
                return _normalAtIntersection;
            }
        }

        // Perturbed normal at intersection point, equals to NormalAtIntersection if no perturbation
        public Vector3 NormalAtIntersection
        {
            get
            {
                _perturbedNormalAtIntersection = _perturbedNormalAtIntersection ?? (SceneObject.Material.Normal == null
                                                                                        ? RawNormalAtIntersection
                                                                                        : SceneObject.Material.Normal.PerturbNormal(this));
                return _perturbedNormalAtIntersection;
            }
        }

        // Reflected vector at intersection point
        public Vector3 ReflectionAtIntersection
        {
            get
            {
                //R = D - 2 * [ N . D ] * N
                _reflectionAtIntersection = _reflectionAtIntersection ?? (Ray.Direction - (2*Vector3.DotProduct(NormalAtIntersection, Ray.Direction))*NormalAtIntersection);
                return _reflectionAtIntersection;
            }
        }
    }
}
