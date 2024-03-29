using System;
using System.Numerics;
using MathUtils;

namespace RayTracer.Engine.Geometries;

public class Torus : GeometryBase
{
    public float MajorRadius { get; private set; }
    public float MinorRadius { get; private set; }

    public Torus(float majorRadius, float minorRadius)
    {
        MajorRadius = majorRadius;
        MinorRadius = minorRadius;
    }

    public override bool HasIntersections(Ray ray)
    {
        return ComputeNearestIntersection(ray, out _);
    }

    public override bool ComputeNearestIntersection(Ray ray, out float t)
    {
        float[] c = new float[5];
        float[] r = new float[4];

        float major2 = MajorRadius*MajorRadius;
        float minor2 = MinorRadius*MinorRadius;

        float od = Vector3.Dot(ray.Origin, ray.Direction);
        float oo = Vector3.Dot(ray.Origin, ray.Origin);
        float k = oo - minor2 - major2;
        
        c[0] = 1.0f;
        c[1] = 4*od;
        c[2] = 2*(2*od*od + k + 2*major2*ray.Direction.Z*ray.Direction.Z);
        c[3] = 4*(k*od + 2*major2*ray.Origin.Z*ray.Direction.Z);
        c[4] = k*k + 4*major2*(ray.Origin.Z*ray.Origin.Z - minor2);

        int n = EquationSolver.SolveQuartic(c, r);
        bool found = GetSmallestPositive(r, n, out t);

        return found;
    }

    public override Vector3 ComputeNormal(Vector3 point)
    {
        float a = 1f - (MajorRadius / (float)Math.Sqrt(point.X * point.X + point.Y * point.Y));
        var normal = new Vector3(point.X * a, point.Y * a, point.Z);
        return Vector3.Normalize(normal);
    }

    public override bool Contains(Vector3 point)
    {
        float t = MajorRadius - (float)Math.Sqrt(point.X*point.X + point.Y*point.Y);
        float f = t*t + point.Z*point.Z - MinorRadius*MinorRadius;
        return f <= Epsilon;
    }

    private static bool GetSmallestPositive(float[] v, int n, out float solution)
    {
        bool found = false;
        solution = float.PositiveInfinity;
        for (int i = 0; i < n; i++)
            if (v[i] > 0.0f && v[i] < solution)
            {
                solution = v[i];
                found = true;
            }
        return found;
    }
}
