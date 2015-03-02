namespace RayTracer
{
    public class Color
    {
        public static readonly Color Black = new Color(0, 0, 0);
        public static readonly Color White = new Color(1, 1, 1);
        public static readonly Color Red = new Color(1, 0, 0);
        public static readonly Color Green = new Color(0, 1, 0);
        public static readonly Color Blue = new Color(0, 0, 1);

        public double R { get; private set; }
        public double G { get; private set; }
        public double B { get; private set; }

        public Color(double r, double g, double b)
        {
            R = r;
            G = g;
            B = b;
        }

        public static Color operator *(Color color, double d)
        {
            return new Color(color.R*d, color.G*d, color.B*d);
        }

        public static Color operator*(Color color1, Color color2)
        {
            return new Color(color1.R*color2.R,color1.G*color2.G,color1.B*color2.B);
        }

        public static Color operator +(Color color1, Color color2)
        {
            return new Color(color1.R + color2.R, color1.G + color2.G, color1.B + color2.B);
        }

        // TODO: blend, clamp

        public override string ToString()
        {
            return string.Format("R:{0:F6} G:{1:F6} B:{2:F6}", R, G, B);
        }
    }
}
