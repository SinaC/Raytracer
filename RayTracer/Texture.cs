using RayTracer.Pigments;

namespace RayTracer
{
    public class Texture
    {
        public Finish Finish { get; private set; }
        public Pigment Pigment { get; private set; }
        // TODO: interior, normal

        public Texture(Finish finish, Pigment pigment)
        {
            Finish = finish;
            Pigment = pigment;
        }
    }
}
