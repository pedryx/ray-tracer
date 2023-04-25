using OpenTK.Mathematics;

using RayTracer.Shapes;

namespace RayTracer.SceneNodes;
/// <summary>
/// Represent pair of shape and it's transformation.
/// </summary>
public class ScenePair
{
    public Shape Shape;
    public Matrix4d Transform;
}
