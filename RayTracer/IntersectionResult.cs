using OpenTK.Mathematics;


namespace RayTracer;
/// <summary>
/// Re[resent a result of intersection between shape and ray.
/// </summary>
public class IntersectResult
{
    /// <summary>
    /// Represent negative intersection result.
    /// </summary>
    public readonly static IntersectResult False = new(
        false,
        double.PositiveInfinity,
        Vector3d.Zero,
        null
    );

    /// <summary>
    /// Determine if ray intersected with shape.
    /// </summary>
    public bool Intersect { get; private set; }
    /// <summary>
    /// Distance from ray position to the intersection point.
    /// </summary>
    public double Distance { get; private set; }
    /// <summary>
    /// Normal at intersection point.
    /// </summary>
    public Vector3d Normal { get; private set; }
    /// <summary>
    /// Material at the intersection point.
    /// </summary>
    public string Material { get; private set; }

    /// <param name="intersect">Determine if ray intersected with shape.</param>
    /// <param name="distance">Distance from ray position to the intersection point.</param>
    /// <param name="normal">Normal at intersection point.</param>
    /// <param name="material">Name of the material at the intersection point.</param>
    public IntersectResult(bool intersect, double distance, Vector3d normal, string material)
    {
        Intersect = intersect;
        Distance = distance;
        Normal = normal;
        Material = material;
    }
}
