using OpenTK.Mathematics;


namespace RayTracer;
/// <summary>
/// Represent ray in 3D space.
/// </summary>
public struct Ray
{
    /// <summary>
    /// Position from where ray begins.
    /// </summary>
    public Vector3d Position;
    /// <summary>
    /// Direction of the ray.
    /// </summary>
    public Vector3d Direction;

    /// <summary>
    /// Get point on ray at specific distance.
    /// </summary>
    public Vector3d At(double distance) => Position + distance * Direction;
}
