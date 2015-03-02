namespace RayTracer
{
    public class Finish
    {
        public static readonly Finish BasicDiffuse = new Finish
            {
                Ambient = 0,
                Diffuse = 1,
                Specular = 0,
                Phong = 0,
                Reflection = 0,
                Transmission = 0,
            };
        public static readonly Finish BasicPhong = new Finish
        {
            Ambient = 0,
            Diffuse = 1,
            Specular = 0,
            Phong = 1,
            PhongSize = 40,
            Reflection = 0,
            Transmission = 0,
        };

        public double Ambient { get; set; } // How ambient affects pigment

        public double Diffuse { get; set; } // How diffuse affects pigment

        //public double Brillance { get; set; } // Tightness of basic diffuse illumination (better if used with specular)

        public double Specular { get; set; } // Specular highlight
        public double SpecularRoughness { get; set; } // Highlight size

        public double Phong { get; set; } // Phong hightlight
        public double PhongSize { get; set; } // Phong size

        public double Reflection { get; set; } // Reflection coefficient
        public double Transmission { get; set; } // Transmittion/Refraction coefficient

        public Finish()
        {
            Ambient = 0.1;
            Diffuse = 0.6;
            //Brillance = 0;
            Specular = 0;
            SpecularRoughness = 20;
            Phong = 0;
            PhongSize = 40;
            Reflection = 0;
            Transmission = 0;
        }
    }
}
