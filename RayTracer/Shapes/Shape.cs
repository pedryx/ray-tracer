using OpenTK.Mathematics;


namespace RayTracer.Shapes;
abstract class Shape
{
    public Color4 Color = Color4.White;

    public abstract Vector3d? Intersect(Ray ray);
}