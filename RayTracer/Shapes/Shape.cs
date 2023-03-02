using OpenTK.Mathematics;


namespace RayTracer.Shapes;
public abstract class Shape
{
    public string Material;
    public Vector3d Position;

    /// <summary>
    /// Computes intersection of ray and shape.
    /// </summary>
    public abstract IntersectResult Intersect(Ray ray);
}