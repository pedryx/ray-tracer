using OpenTK.Mathematics;


namespace RayTracer;
/// <summary>
/// Represent a material for Phong Shading Model.
/// </summary>
class Material
{
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
    public Vector3d Color;
}
