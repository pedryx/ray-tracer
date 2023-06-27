using OpenTK.Mathematics;

using RayTracer.Shapes;

namespace RayTracer.SceneNodes;
/// <summary>
/// Represent leaf node (shape) with accumulated attributes from parent nodes in scene graph.
/// </summary>
public class ShapeNode
{
    public Shape Shape { get; private set; }
    public Matrix4d Transform { get; private set; }
    public string Material { get; private set; }

    public ShapeNode(Shape shape, Matrix4d transform, string material)
    {
        Shape = shape;
        Transform = transform;
        Material = material;
    }

    /// <summary>
    /// Computes intersection between ray and current scene tree leaf node.
    /// </summary>
    /// <param name="ray"></param>
    /// <returns></returns>
    public IntersectResult Intersect(Ray ray)
    {
        IntersectResult result = Shape.Intersect(ray);

        return new IntersectResult(result, Material);
    }
}
