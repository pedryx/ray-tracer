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
    public bool Shadows = true;
    public bool Reflections = true;
    public bool Refractions = true;
    public int MaxDepth = 8;
}
