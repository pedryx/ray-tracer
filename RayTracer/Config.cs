namespace RayTracer;
/// <summary>
/// Represent program configuration.
/// </summary>
public class Config
{
    /// <summary>
    /// File where output image should be stored.
    /// </summary>
    public string OutputFile { get; init; } = "demo.pfm";
    /// <summary>
    /// Definition of scene objects.
    /// </summary>
    public SceneDefinition Scene { get; init; } = new();
    public Camera Camera { get; init; } = new();
    /// <summary>
    /// Determine if shadows are enabled.
    /// </summary>
    public bool Shadows { get; init; } = true;
    /// <summary>
    /// Determine if reflections are enabled.
    /// </summary>
    public bool Reflections { get; init; } = true;
    /// <summary>
    /// Determine if refractions are enabled.
    /// </summary>
    public bool Refractions { get; init; } = true;
    /// <summary>
    /// Maximum recursion depth.
    /// </summary>
    public int MaxDepth { get; init; } = 5;
    public int SamplesPerPixel { get; init; } = 16;
}
