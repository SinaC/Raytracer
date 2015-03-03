namespace RayTracer
{
    public class Interior
    {
        public const double Air = 1.000293;
        public const double Vacuum = 1.0;
        public const double Water = 1.333;
        public const double Ice = 1.309;
        public const double Glass = 1.48749;

        public double IndexOfRefraction { get; set; }
        //TODO: public Color Absorption { get; set; }

        public Interior()
        {
            IndexOfRefraction = Vacuum;
        }
    }
}
