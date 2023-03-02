using OpenTK.Mathematics;

using RayTracer.Utils;


namespace RayTracer;
/// <summary>
/// Represent scene.
/// </summary>
class Scene
{
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
            Ray ray = config.Camera.CreateRay(new Vector2d(x, y));

            // check for ray intersaction
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

            // no intersection so using background color
            if (!nearestHit.Intersect)
                return null;

            Material material = config.Scene.Materials[nearestHit.Material];

            // compute light intesinty
            Vector3d point = ray.At(nearestHit.Distance);
            Vector3d intensity = Vector3d.Zero;
            foreach (var source in config.Scene.LightSources)
            {
                intensity += source.Reflectance
                (
                    nearestHit.Normal,
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
}