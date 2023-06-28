namespace RayTracer;
/// <summary>
/// Represent program configuration.
/// </summary>
public class Config
{
    /// <summary>
    /// File where output image should be stored.
    /// </summary>
    public string OutputFile = "demo.pfm";
    /// <summary>
    /// Definition of scene objects.
    /// </summary>
    public SceneDefinition Scene = new();
    public Camera Camera;
    /// <summary>
    /// Determine if shadows are enabled.
    /// </summary>
    public bool Shadows = true;
    /// <summary>
    /// Determine if reflections are enabled.
    /// </summary>
    public bool Reflections = true;
    /// <summary>
    /// Determine if refractions are enabled.
    /// </summary>
    public bool Refractions = true;
    /// <summary>
    /// Maximum recursion depth.
    /// </summary>
    public int MaxDepth = 8;
}
