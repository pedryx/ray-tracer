using OpenTK.Mathematics;

using RayTracer.Shapes;

namespace RayTracer;
/// <summary>
/// Represent ray in 3D space.
/// </summary>
public readonly struct Ray
{
    /// <summary>
    /// Position from where ray begins.
    /// </summary>
    public Vector3d Position { get; init; }
    /// <summary>
    /// Direction of the ray.
    /// </summary>
    public Vector3d Direction { get; init; }

    /// <summary>
    /// Shape fro which ray originates, or null.
    /// </summary>
    public Shape Shape { get; init; }

    public Ray(Vector3d position, Vector3d direction, Shape shape = null)
    {
        Position = position;
        Direction = direction;
        Shape = shape;
    }

    public Ray(Ray ray, Shape shape = null)
    {
        Position = ray.Position;
        Direction = ray.Direction;
        Shape = shape;
    }

    /// <summary>
    /// Get point on ray at specific distance.
    /// </summary>
    public readonly Vector3d At(double distance) => Position + distance * Direction;
}
