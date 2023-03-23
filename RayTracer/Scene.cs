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
        var image = new FloatImage((int)config.Camera.Resolution.X, (int)config.Camera.Resolution.Y, 3);

        // render background
        image.ForEach((x, y) => config.Scene.BackgroundColor);

        // render scene
        image.ForEach((x, y) =>
        {
            // create ray and check for intersection
            Ray ray = config.Camera.CreateRay(new Vector2d(x, y));
            IntersectResult result = Intersect(ray);

            // no intersection so using background color
            if (!result.Intersect)
                return null;

            // check for shadow

            Material material = config.Scene.Materials[result.Material];

            // compute light intesinty
            Vector3d point = ray.At(result.Distance);
            Vector3d intensity = Vector3d.Zero;
            foreach (var source in config.Scene.LightSources)
            {
                if (source is PointLightSource pointLightSource && InShadow(point, pointLightSource))
                    continue;

                intensity += source.Reflectance
                (
                    result.Normal,
                    point,
                    config.Camera.Position,
                    material
                );
            }
            intensity = Vector3d.Clamp(intensity, Vector3d.Zero, Vector3d.One);
            
            // compute pixel color
            return (Vector3)intensity * material.Color;
        });

        image.SavePFM(config.OutputFile);
        Logger.WriteLine("HDR image is finished.", LogType.Message);
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
        var ray = new Ray()
        {
            Position = point,
            Direction = (source.Position - point).Normalized(),
        };
        ray.Position += ray.Direction * threshold;

        IntersectResult result = Intersect(ray);
        double maxDistance = Vector3d.Distance(source.Position, point);

        return result.Intersect && result.Distance < maxDistance;
    }
}