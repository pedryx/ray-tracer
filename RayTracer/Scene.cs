using OpenTK.Mathematics;

using RayTracer.Utils;


namespace RayTracer;
/// <summary>
/// Represent scene.
/// </summary>
class Scene
{
    private const double threshold = 1e-6;

    private readonly Config config;

    public Scene(Config config)
    {
        this.config = config;
    }

    /// <summary>
    /// Render scene to the image according to the configuration.
    /// </summary>
    public void Render()
    {
        // create image
        var image = new FloatImage((int)config.Camera.Resolution.X, (int)config.Camera.Resolution.Y, 3);

        // render background
        image.ForEach((x, y) => config.Scene.BackgroundColor);

        // render scene
        image.ForEach((x, y) =>
        {
            Ray ray = config.Camera.CreateRay(new Vector2d(x, y));
            return Shade(ray);
        });

        // save image
        image.SavePFM(config.OutputFile);
        Logger.WriteLine("HDR image is finished.", LogType.Message);
    }

    /// <summary>
    /// Compute color for ray casted into the scene.
    /// </summary>
    /// <param name="ray">Casted ray.</param>
    /// <returns>Computed color, or null if ray dont intersect.</returns>
    private Color? Shade(Ray ray)
    {
        // check for intersection
        IntersectResult result = Intersect(ray);

        // no intersection so using background color
        if (!result.Intersect)
            return null;

        // compute light intesinty
        Material material = config.Scene.Materials[result.Material];
        Vector3d point = ray.At(result.Distance);
        Vector3d intensity = ComputeLightIntensity(material, point, result.Normal);

        // compute pixel color
        return (Vector3)intensity * material.Color;
    }

    /// <summary>
    /// CHeck for intersection between ray and scene.
    /// </summary>
    private IntersectResult Intersect(Ray ray)
    {
        IntersectResult nearestHit = IntersectResult.False;
        foreach (var shape in config.Scene.Shapes)
        {
            var result = shape.Intersect(ray);
            if (result.Intersect)
            {
                if (result.Distance < nearestHit.Distance)
                    nearestHit = result;
            }
        }

        return nearestHit;
    }

    /// <summary>
    /// Check if specific point is in shadow for specific light source.
    /// </summary>
    /// <param name="point">point to check.</param>
    private bool InShadow(Vector3d point, PointLightSource source)
    {
        // compute ray from intersection point towards light source
        var ray = new Ray()
        {
            Position = point,
            Direction = (source.Position - point).Normalized(),
        };
        ray.Position += ray.Direction * threshold;

        // compute intersection with scene
        IntersectResult result = Intersect(ray);
        double maxDistance = Vector3d.Distance(source.Position, point);

        return result.Intersect && result.Distance < maxDistance;
    }

    /// <summary>
    /// Compute light intensity at specific point.
    /// </summary>
    /// <param name="material">Material of shape on which is point located.</param>
    /// <param name="point">Point for which will be intensity computed.</param>
    /// <param name="normal">Normal at specific point.</param>
    /// <returns>Computed light intensity at specific point.</returns>
    private Vector3d ComputeLightIntensity(Material material, Vector3d point, Vector3d normal)
    {
        Vector3d intensity = Vector3d.Zero;
        
        foreach (var source in config.Scene.LightSources)
        {
            // check for shadow
            if (config.Shadows)
            {
                if (source is PointLightSource pointLightSource && InShadow(point, pointLightSource))
                    continue;
            }

            // compute intensity
            intensity += source.Reflectance
            (
                normal,
                point,
                config.Camera.Position,
                material
            );
        }

        intensity = Vector3d.Clamp(intensity, Vector3d.Zero, Vector3d.One);
        return intensity;
    }
}