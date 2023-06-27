using OpenTK.Mathematics;

using RayTracer.Utils;

using System;
using System.Diagnostics;

namespace RayTracer;
/// <summary>
/// Represent scene.
/// </summary>
class Scene
{
    private const double threshold = 1e-6;

    private readonly Config config;
    private readonly SceneGraph graph;

    public Scene(Config config, SceneGraph graph)
    {
        this.config = config;
        this.graph = graph;
    }

    /// <summary>
    /// Render scene to the image according to the configuration.
    /// </summary>
    public void Render()
    {
        // prepare and start the timer
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // create image
        var image = new FloatImage((int)config.Camera.Resolution.X, (int)config.Camera.Resolution.Y, 3);

        // prepare shapes
        foreach (var node in graph)
        {
            node.Shape.Transform(node.Transform);
        }

        // render scene
        image.ForEach((x, y) =>
        {
            Ray ray = config.Camera.CreateRay(new Vector2d(x, y));
            return Shade(ray, config.MaxDepth);
        });
        stopwatch.Stop();

        // save image
        image.SavePFM(config.OutputFile);
        Logger.WriteLine("HDR image is finished.", LogType.Message);
        Logger.WriteLine($"Done in {stopwatch.ElapsedMilliseconds}ms.", LogType.Message);
    }

    /// <summary>
    /// Compute color for ray casted into the scene.
    /// </summary>
    /// <param name="ray">Casted ray.</param>
    /// <returns>Computed color, or null if ray dont intersect.</returns>
    private Color Shade(Ray ray, int depth)
    {
        // result color
        Color color = new(0, 0, 0);

        // check for intersection
        IntersectResult result = Intersect(ray);

        // no intersection so using background color
        if (!result.Intersect)
            return config.Scene.BackgroundColor;
        
        // compute light intensity
        if (!config.Scene.Materials.TryGetValue(result.Material.ToLower(), out Material material))
        {
            string message = $"Material \"{result.Material.ToLower()}\" is not defined.";
            Logger.WriteLine(message, LogType.Error);
            throw new Exception(message);
        }

        Vector3d point = ray.At(result.Distance);
        Vector3d intensity = ComputeLightIntensity(material, point, result.Normal);
        color += (Vector3)intensity * material.Color;

        // depth check
        if (depth == 0)
            return color;

        // compute reflection
        if (config.Reflections)
        {
            Ray reflectionRay = Reflection(result.Normal.Normalized(), ray.Direction.Normalized(), point);
            color += 0.5f * Shade(reflectionRay, depth - 1);
        }

        return color;
    }

    /// <summary>
    /// Compute reflection unit vector.
    /// </summary>
    /// <param name="normal">Shape's normal.</param>
    /// <param name="direction">Ray's direction.</param>
    /// <param name="point">Intersection point.</param>
    /// <returns>Reflection unit vector.</returns>
    private static Ray Reflection(Vector3d normal, Vector3d direction, Vector3d point)
    {
        Ray ray = new(point, direction - 2 * Vector3d.Dot(direction, normal) * normal);
        ray.Position += ray.Direction * threshold;
        ray.Direction.Normalize();

        return ray;
    }

    /// <summary>
    /// CHeck for intersection between ray and scene.
    /// </summary>
    private IntersectResult Intersect(Ray ray)
    {
        IntersectResult nearestHit = IntersectResult.False;
        foreach (var node in graph)
        {
            var result = node.Intersect(ray);
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