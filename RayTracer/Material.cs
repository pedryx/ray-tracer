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
    public double Highlight;
    public double Reflection;
    public double Refraction;
    public Color Color;
}
