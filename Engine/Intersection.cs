using System.Numerics;

namespace RayTracer.Engine;

public class Intersection
{
    public static Intersection NullObject { get; } = new(null, null, float.MinValue);

    public SceneObject SceneObject { get; private set; }
    public Ray Ray { get; private set; }
    public float T { get; private set; }

    private Vector3? _intersectionPoint; // cache computed value
    private Vector3? _normalAtIntersection; // cache computed value
    private Vector3? _reflectionAtIntersection; // cache computed value
    private Vector3? _perturbedNormalAtIntersection; // cache computed value

    public Intersection(SceneObject sceneObject, Ray ray, float t)
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
            _intersectionPoint ??= Ray.Origin + Ray.Direction*T;
            return _intersectionPoint.Value;
        }
    }

    // Normal (non-perturbed) at intersection point
    public Vector3 RawNormalAtIntersection
    {
        get
        {
            _normalAtIntersection ??= SceneObject.Geometry.ComputeNormal(IntersectionPoint);
            return _normalAtIntersection.Value;
        }
    }

    // Perturbed normal at intersection point, equals to NormalAtIntersection if no perturbation
    public Vector3 NormalAtIntersection
    {
        get
        {
            _perturbedNormalAtIntersection ??= (SceneObject.Material.Normal == null
                                                                                    ? RawNormalAtIntersection
                                                                                    : SceneObject.Material.Normal.PerturbNormal(this));
            return _perturbedNormalAtIntersection.Value;
        }
    }

    // Reflected vector at intersection point
    public Vector3 ReflectionAtIntersection
    {
        get
        {
            // R  = D - 2 * [ N . D ] * N
            _reflectionAtIntersection ??= (Ray.Direction - (2*Vector3.Dot(NormalAtIntersection, Ray.Direction))*NormalAtIntersection);
            return _reflectionAtIntersection.Value;
        }
    }
}
