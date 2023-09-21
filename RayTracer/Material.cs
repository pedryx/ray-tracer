namespace RayTracer;
/// <summary>
/// Represent a material for Phong Shading Model.
/// </summary>
public class Material
{
    public string Name { get; init; }
    /// <summary>
    /// Ambient coefficient.
    /// </summary>
    public double Ambient { get; init; }
    /// <summary>
    /// Diffuse coefficient.
    /// </summary>
    public double Diffuse { get; init; }
    /// <summary>
    /// Specular Coefficient.
    /// </summary>
    public double Specular { get; init; }
    /// <summary>
    /// Highlight used to specular computation.
    /// </summary>
    public double Highlight { get; init; }
    /// <summary>
    /// How much lught should be reflected.
    /// </summary>
    public double Reflection { get; init; }
    /// <summary>
    /// How much light shoud be refracted.
    /// </summary>
    public double Refraction { get; init; }
    public double RefractiveIndex { get; init; }
    public Color Color {  get; init; }
}
