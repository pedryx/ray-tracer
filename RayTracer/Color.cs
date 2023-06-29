using OpenTK.Mathematics;

using System.Drawing;

namespace RayTracer;
/// <summary>
/// Represent a RGB Color.
/// </summary>
public struct Color
{
    private Vector3 vector;

    public float R
    {
        get => vector.X;
        set => vector.X = value;
    }

    public float G
    {
        get => vector.Y;
        set => vector.Y = value;
    }

    public float B
    {
        get => vector.Z;
        set=> vector.Z = value;
    }

    public Color(Vector3 vector)
    {
        this.vector = vector;
    }

    public Color(float r, float g, float b)
    {
        R = r;
        G = g;
        B = b;
    }

    public static Color operator *(Color color, float k)
        => new(color.vector * k);
    public static Color operator *(float k, Color color)
        => new(color.vector * k);
    public static Color operator /(Color color, float k)
        => new(color.vector / k);
    public static Color operator *(Color color1, Color color2)
        => new(color1.vector * color2.vector);
    public static Color operator +(Color color1, Color color2)
        => new(color1.vector + color2.vector);
    public static Color operator *(Color color, Vector3 vector)
        => new(color.vector * vector);
    public static Color operator *(Vector3 vector, Color color)
        => new(color.vector * vector);
}
