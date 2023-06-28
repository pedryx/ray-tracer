namespace RayTracer;
/// <summary>
/// Represent a material for Phong Shading Model.
/// </summary>
public class Material
{
    public string Name;
    /// <summary>
    /// Ambient coefficient.
    /// </summary>
    public double Ambient;
    /// <summary>
    /// Diffuse coefficient.
    /// </summary>
    public double Diffuse;
    /// <summary>
    /// Specular Coefficient.
    /// </summary>
    public double Specular;
    /// <summary>
    /// Highlight used to specular computation.
    /// </summary>
    public double Highlight;
    /// <summary>
    /// How much lught should be reflected.
    /// </summary>
    public double Reflection;
    /// <summary>
    /// How much light shoud be refracted.
    /// </summary>
    public double Refraction;
    public double RefractiveIndex;
    public Color Color;
}
