using OpenTK.Mathematics;

using RayTracer.Shapes;
using RayTracer.Utils;

using System.Collections.Generic;


namespace RayTracer;
class Scene
{
    private readonly Camera camera;
    private readonly Config config;
    private readonly List<Shape> shapes = new();
    private readonly Dictionary<string, Material> materials = new();
    private readonly List<LightSource> lightSources = new();

    public Scene(Config config)
    {
        this.config = config;

        camera = new Camera(config.ImageWidth, config.ImageHeight)
        {
            Position = new Vector3d(0.6, 0, -5.6),
            Direction = new Vector3d(0, -0.03, 1),
            FOV = 40,
        };
    }

    public void CreateSolids()
    {
        materials["Yellow"] = new Material()
        {
            Ambient = 0.1,
            Diffuse = 0.8,
            Specular = 0.2,
            Highlight = 10,
            Color = new Vector3d(1, 1, 0.2),
        };
        materials["Blue"] = new Material()
        {
            Ambient = 0.1,
            Diffuse = 0.5,
            Specular = 0.5,
            Highlight = 150,
            Color = new Vector3d(0.2, 0.3, 1.0),
        };
        materials["Red"] = new Material()
        {
            Ambient = 0.1,
            Diffuse = 0.6,
            Specular = 0.4,
            Highlight = 80,
            Color = new Vector3d(0.8, 0.2, 0.2),
        };

        lightSources.Add(new AmbientLightSource()
        {
            Intensity = new Vector3d(1, 1, 1),
        });
        lightSources.Add(new PointLightSource()
        {
            Position = new Vector3d(-10, 8, -6),
            Intensity = new Vector3d(1, 1, 1),
        });
        lightSources.Add(new PointLightSource()
        {
            Position = new Vector3d(0, 20, -3),
            Intensity = new Vector3d(0.3, 0.3, 0.3),
        });

        shapes.Add(new Sphere()
        {
            Position = new Vector3d(0, 0, 0),
            Radius = 1,
            Material = materials["Yellow"],
        });
        shapes.Add(new Sphere()
        {
            Position = new Vector3d(1.4, -0.7, -0.5),
            Radius = 0.6,
            Material = materials["Blue"],
        });
        shapes.Add(new Sphere()
        {
            Position = new Vector3d(-0.7, 0.7, -0.8),
            Radius = 0.1,
            Material = materials["Red"],
        });
        shapes.Add(new Plane()
        {
            Position = new Vector3d(0, -1.5, 0),
            Normal = Vector3d.UnitY,
            Material = materials["Red"],
        });
    }

    public void Render()
    {
        var image = new FloatImage(config.ImageWidth, config.ImageHeight, 3);

        // render background
        // 135 206 235
        Vector3d backgroundColor = new Vector3d(0.1, 0.2, 0.3);
        image.ForEach((x, y) => backgroundColor);

        // render scene
        image.ForEach((x, y) =>
        {
            Ray ray = camera.CreateRay(new Vector2d(x, y));

            // check for ray intersaction
            IntersectResult nearestHit = IntersectResult.False;
            foreach (var shape in shapes)
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

            // compute pixel color
            Vector3d point = ray.At(nearestHit.Distance);
            Vector3d intensity = Vector3d.Zero;
            foreach (var source in lightSources)
            {
                intensity += source.Reflectance(nearestHit.Normal, point, nearestHit.Material);
            }
            intensity = Vector3d.Clamp(intensity, Vector3d.Zero, Vector3d.One);
            
            return intensity * nearestHit.Material.Color;
        });

        image.SavePFM(config.OutputFile);
        Logger.WriteLine("HDR image is finished.", LogType.Message);
    }
}