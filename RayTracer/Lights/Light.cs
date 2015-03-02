namespace RayTracer.Lights
{
    public abstract class Light
    {
        public abstract Color GetColor(Intersection intersection, Scene scene);
    }
}
