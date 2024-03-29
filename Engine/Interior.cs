namespace RayTracer.Engine;

public class Interior
{
    public const float Air = 1.000293f;
    public const float Vacuum = 1.0f;
    public const float Water = 1.333f;
    public const float Ice = 1.309f;
    public const float Glass = 1.48749f;

    public float IndexOfRefraction { get; init; }
    //TODO: public Color Absorption { get; set; }

    public Interior()
    {
        IndexOfRefraction = Vacuum;
    }
}
