using System.Threading;

namespace RayTracer.Engine;

public class Color
{
    private static int _allocationCount;
    public static int AllocationCount => _allocationCount;

    public static Color Black { get; } = new Color(0, 0, 0);
    public static Color White { get; } = new Color(1, 1, 1);
    public static Color Red { get; } = new Color(1, 0, 0);
    public static Color Green { get; } = new Color(0, 1, 0);
    public static Color Blue { get; } = new Color(0, 0, 1);
    public static Color Yellow { get; } = new Color(1, 1, 0);
    public static Color Magenta { get; } = new Color(1, 0, 1);
    public static Color Cyan { get; } = new Color(0, 1, 1);

    public float R { get; init; }
    public float G { get; init; }
    public float B { get; init; }

    public Color(float r, float g, float b)
    {
        Interlocked.Increment(ref _allocationCount); 
        
        R = r;
        G = g;
        B = b;
    }

    public Color(Color color)
        : this(color.R, color.G, color.B)
    {
    }

    //public void Times(float d)
    //{
    //    R *= d;
    //    G *= d;
    //    B *= d;
    //}

    //public void Times(Color color)
    //{
    //    R *= color.R;
    //    G *= color.G;
    //    B *= color.B;
    //}

    //public void Add(Color color)
    //{
    //    R += color.R;
    //    G += color.G;
    //    B += color.B;
    //}

    public static Color operator *(Color color, float d)
    {
        return new Color(color.R * d, color.G * d, color.B * d);
    }

    public static Color operator *(float d, Color color)
    {
        return new Color(color.R * d, color.G * d, color.B * d);
    }

    public static Color operator *(Color color1, Color color2)
    {
        return new Color(color1.R * color2.R, color1.G * color2.G, color1.B * color2.B);
    }

    public static Color operator +(Color color1, Color color2)
    {
        return new Color(color1.R + color2.R, color1.G + color2.G, color1.B + color2.B);
    }

    // TODO: blend, clamp

    public override string ToString()
        => string.Format("R:{0:F6} G:{1:F6} B:{2:F6}", R, G, B);
}
