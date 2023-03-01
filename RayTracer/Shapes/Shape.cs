using OpenTK.Mathematics;


namespace RayTracer.Shapes;
abstract class Shape
{
    public Material Material;

    /// <summary>
    /// Computes intersection of ray and shape.
    /// </summary>
    public abstract IntersectResult Intersect(Ray ray);
}