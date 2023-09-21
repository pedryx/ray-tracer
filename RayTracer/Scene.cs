using OpenTK.Mathematics;

using RayTracer.Shapes;
using RayTracer.Utils;

using SimplexNoise;

using System;
using System.Diagnostics;

namespace RayTracer;
/// <summary>
/// Represent scene.
/// </summary>
class Scene
{
    private const int progressBarStep = 20000;

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
        var random = new Random();
        int regions = (int)Math.Sqrt(config.SamplesPerPixel);
        double size = 1.0 / regions;
        object progressLock = new();
        int pixelsDone = 0;
        int progressBarSize = (int)((config.Camera.Resolution.X * config.Camera.Resolution.Y) / progressBarStep) + 1;
        Console.Write($"Progress({progressBarSize}): ");
        image.ForEach((x, y) =>
        {
            Color colorSum = new();
            for (int rx = 0; rx < regions; rx++)
            {
                for (int ry = 0; ry < regions; ry++)
                {
                    var position = new Vector2d()
                    {
                        X = x + rx * size * random.NextDouble(),
                        Y = y + ry * size * random.NextDouble(),
                    };
                    Ray ray = config.Camera.CreateRay(position);
                    colorSum += Shade(ray, config.MaxDepth);
                }
            }

            lock (progressLock)
            {
                pixelsDone++;
                if (pixelsDone % progressBarStep == 0)
                    Console.Write('#');
            }

            return colorSum / config.SamplesPerPixel;
        });
        Console.WriteLine('#');
        stopwatch.Stop();

        // save image
        image.SavePFM(config.OutputFile);
        Logger.WriteLine("HDR image is finished.", LogType.Message);
        Logger.WriteLine($"Done in {stopwatch.ElapsedMilliseconds}ms.", LogType.Message);
    }

    private static float CalcNoise(Vector3d position, float scale)
        => Noise.CalcPixel3D((int)position.X, (int)position.Y, (int)position.Z, scale) / 255.0f;

    private Color CalcBackground(Ray ray)
    {
        const float skyDistance = 1000.0f;
        Vector3d position = ray.At(skyDistance);

        if (!config.Scene.Clouds)
            return config.Scene.BackgroundColor;

        float value = 1.0f * CalcNoise(position, 0.005f)
            + 0.4f * CalcNoise(position, 0.01f);
        value = (value + 1.4f) / 2.8f;
        value = value * value * value * value * value;

        Color result = Color.Lerp(config.Scene.BackgroundColor, new Color(1.0f, 1.0f, 1.0f), value);

        return result;
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
            return CalcBackground(ray);

        // get material
        if (!config.Scene.Materials.TryGetValue(result.Material.ToLower(), out Material material))
        {
            string message = $"Material \"{result.Material.ToLower()}\" is not defined.";
            Logger.WriteLine(message, LogType.Error);
            throw new Exception(message);
        }

        // compute light intensity
        Vector3d point = ray.At(result.Distance);
        Vector3d intensity = ComputeLightIntensity(material, result.Shape, point, result.Normal);
        color += (Vector3)intensity * material.Color;

        // depth check
        if (depth == 0)
            return color;

        // compute reflection
        if (config.Reflections)
        {
            Ray reflectionRay = Reflection(result.Normal.Normalized(), ray.Direction.Normalized(), point);
            color += (float)material.Reflection * Shade(new Ray(reflectionRay, result.Shape), depth - 1);
        }

        // compute refraction
        if (config.Refractions)
        {
            double refractiveIndex = result.FrontFace ? 1.0 / material.RefractiveIndex : material.RefractiveIndex;
            Ray refractionRay = Refraction(result.Normal.Normalized(), ray.Direction.Normalized(), point, refractiveIndex);
            if (refractionRay.Direction != Vector3d.Zero)
                color += (float)material.Refraction * Shade(new Ray(refractionRay, result.Shape), depth - 1);
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
        => new(point, direction - 2 * Vector3d.Dot(direction, normal) * normal);

    /// <summary>
    /// Compute refraction unit vector.
    /// </summary>
    /// <param name="normal">Shape's normal.</param>
    /// <param name="direction">Ray's direction.</param>
    /// <param name="point">Intersection point.</param>
    /// <param name="refractionIndex">Shape's refraction index.</param>
    /// <returns>Refractio unit vector.</returns>
    private static Ray Refraction(Vector3d normal, Vector3d direction, Vector3d point, double refractionIndex)
    {
        double dot = Math.Min(Vector3d.Dot(-direction, normal), 1.0);
        if (refractionIndex * Math.Sqrt(1.0 - dot * dot) > 1.0)
            return new(point, Vector3d.Zero);
        Vector3d temp = refractionIndex * (direction + dot * normal);
        Vector3d t = temp - Math.Sqrt(Math.Abs(1.0 - temp.LengthSquared)) * normal;
        return new(point, t);
    }

    /// <summary>
    /// CHeck for intersection between ray and scene.
    /// </summary>
    private IntersectResult Intersect(Ray ray)
    {
        IntersectResult nearestHit = IntersectResult.False;
        foreach (var node in graph)
        {
            if (ray.Shape == node.Shape)
                continue;

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
    private bool InShadow(Vector3d point, Shape shape, PointLightSource source)
    {
        // compute ray from intersection point towards light source
        var ray = new Ray()
        {
            Position = point,
            Direction = source.Position - point,
            Shape = shape,
        };

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
    private Vector3d ComputeLightIntensity(Material material, Shape shape, Vector3d point, Vector3d normal)
    {
        Vector3d intensity = Vector3d.Zero;
        
        foreach (var source in config.Scene.LightSources)
        {
            // check for shadow
            if (config.Shadows)
            {
                if (source is PointLightSource pointLightSource && InShadow(point, shape, pointLightSource))
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