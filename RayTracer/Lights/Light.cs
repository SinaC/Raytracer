using MathUtils;

namespace RayTracer.Lights
{
    public abstract class Light
    {
        public abstract bool IsLighten(Vector3 point);
        public abstract Color GetColor(Intersection intersection, Scene scene);
   }
}
