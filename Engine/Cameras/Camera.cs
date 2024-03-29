using System;
using System.Numerics;

namespace RayTracer.Engine.Cameras;

public class Camera
{
    public Vector3 Eye { get; private set; }
    public Vector3 LookAt { get; private set; }
    public Vector3 View { get; private set; }
    public Vector3 Up { get; private set; }
    public Vector3 Right { get; private set; }

    public float Fov { get; private set; } // in degrees
    public float Aspect { get; private set; } // screen X-Y relation

    private readonly float _tanFov;

    public Camera(Vector3 eye, Vector3 lookAt)
    {
        Eye = eye;
        LookAt = lookAt;
        Up = new Vector3(0, 1, 0);

        Fov = 45;
        _tanFov = (float)Math.Tan(Fov*Math.PI/180.0f);
        Aspect = 4/3;

        Recalculate();
    }

    private void Recalculate()
    {
        var view = LookAt - Eye;
        View = Vector3.Normalize(view);

        var right = Vector3.Cross(View, Up);
        Right = Vector3.Normalize(right);
        var up = Vector3.Cross(Right, View); // re-base the up vector
        Up = Vector3.Normalize(up);
    }

    public Vector3 ComputeRayDirection(float x, float y)
    {
        // image point = O + V + R * x + Up * Y
        // direction = image point - O
        Vector3 dir = View
                      + (Right*(_tanFov*Aspect*x))
                      + (Up*(_tanFov*y));
        return Vector3.Normalize(dir);
    }

    // TODO: move this to a Screen/Renderer class
    public Vector3 ComputeRayDirection(int screenWidth, int screenHeight, int x, int y)
    {
        // -0.5 to cast in the center of the pixel
        return ComputeRayDirection((float)x/screenWidth - 0.5f, (float) y/screenHeight - 0.5f);
    }
}
