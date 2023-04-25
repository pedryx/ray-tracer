using OpenTK.Mathematics;

namespace RayTracer.Shapes;
/// <summary>
/// Base class for shapes.
/// </summary>
public abstract class Shape
{
    /// <summary>
    /// Position of the shape in the scene. (Can be affected be transformations.)
    /// </summary>
    public Vector3d Position;

    /// <summary>
    /// Computes intersection of ray and shape.
    /// </summary>
    public abstract IntersectResult Intersect(Ray ray);
}