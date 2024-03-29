namespace RayTracer.Engine;

public class Finish
{
    public static Finish BasicDiffuse { get; } = new()
    {
        Ambient = 0,
        Diffuse = 1,
        Specular = 0,
        Phong = 0,
        Reflection = 0,
        Transmission = 0,
    };

    public static Finish BasicPhong { get; } = new()
    {
        Ambient = 0,
        Diffuse = 1,
        Specular = 0,
        Phong = 1,
        PhongSize = 40,
        Reflection = 0,
        Transmission = 0,
    };

    public float Ambient { get; init; } // How ambient affects pigment

    public float Diffuse { get; init; } // How diffuse affects pigment

    //public float Brillance { get; init; } // Tightness of basic diffuse illumination (better if used with specular)

    public float Specular { get; init; } // Specular highlight
    public float SpecularRoughness { get; init; } // Highlight size

    public float Phong { get; init; } // Phong hightlight
    public float PhongSize { get; init; } // Phong size

    public float Reflection { get; init; } // Reflection coefficient
    public float Transmission { get; init; } // Transmittion/Refraction coefficient

    public Finish()
    {
        Ambient = 0.1f;
        Diffuse = 0.6f;
        //Brillance = 0;
        Specular = 0;
        SpecularRoughness = 20;
        Phong = 0;
        PhongSize = 40;
        Reflection = 0;
        Transmission = 0;
    }
}
