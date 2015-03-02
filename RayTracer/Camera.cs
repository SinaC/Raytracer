using System;
using MathUtils;

namespace RayTracer
{
    //http://stackoverflow.com/questions/13078243/ray-tracing-camera
    //http://stackoverflow.com/questions/12892906/generate-a-vector/12892966#12892966
    public class Camera
    {
        public Vector3 Eye { get; private set; }
        public Vector3 LookAt { get; private set; }
        public Vector3 View { get; private set; }
        public Vector3 Up { get; private set; }
        public Vector3 Right { get; private set; }

        public double Fov { get; private set; } // in degrees
        public double Aspect { get; private set; } // X-Y relation

        private readonly double _tanFov;

        public Camera(Vector3 eye, Vector3 lookAt)
        {
            Eye = eye;
            LookAt = lookAt;
            Up = new Vector3(0, 1, 0);

            Fov = 45;
            _tanFov = Math.Tan(Fov*Math.PI/180.0);
            Aspect = 4/3;

            Recalculate();
        }

        private void Recalculate()
        {
            View = LookAt - Eye;
            View.Normalize();

            Right = Vector3.CrossProduct(View, Up); // no need to normalize
            Up = Vector3.CrossProduct(Right, View); // re-base the up vector
        }

        public Vector3 ComputeRayDirection(double x, double y)
        {
            // image point = O + V + R * x + Up * Y
            // direction = image point - O
            Vector3 dir = View
                          + (Right*(_tanFov*Aspect*x))
                          + (Up*(_tanFov*y));
            dir.Normalize();
            return dir;
        }

        // TODO: move this to a Screen class
        public Vector3 ComputeRayDirection(int screenWidth, int screenHeight, int x, int y)
        {
            // -0.5 to cast in the center of the pixel
            return ComputeRayDirection((double) x/screenWidth - 0.5, (double) y/screenHeight - 0.5);
        }
    }
}
