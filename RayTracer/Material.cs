namespace RayTracer;
/// <summary>
/// Represent a material for Phong Shading Model.
/// </summary>
public class Material
{
    public string Name;
    /// <summary>
    /// Ambient coeficient.
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
    public Color Color;
}
