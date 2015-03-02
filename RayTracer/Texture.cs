using RayTracer.Normals;
using RayTracer.Pigments;

namespace RayTracer
{
    public class Texture
    {
        public Finish Finish { get; private set; }
        public Pigment Pigment { get; private set; }
        public Normal Normal { get; private set; }
        public Interior Interior { get; private set; }

        public Texture(Finish finish, Pigment pigment, Normal normal = null, Interior interior = null)
        {
            Finish = finish;
            Pigment = pigment;
            Normal = normal;
            Interior = interior;
        }
    }
}
