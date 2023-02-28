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

    public Scene(Config config)
    {
        this.config = config;

        camera = new Camera(config.ImageWidth, config.ImageHeight)
        {
            Position = new Vector3(0, 0, 0),
        };
    }

    public void CreateSolids()
    {
        shapes.Add(new Sphere()
        {
            Position = new Vector3d(-1, 0, 10),
            Radius = 1,
            Color = Color4.Red,
        });
        shapes.Add(new Sphere()
        {
            Position = new Vector3d(1, 0, 10),
            Radius = 2,
            Color = Color4.Blue,
        });
    }

    public void Render()
    {
        var image = new FloatImage(config.ImageWidth, config.ImageHeight, 3);

        // render background
        Color4 backgroundColor = Color4.SkyBlue;
        image.ForEach((x, y) => backgroundColor);

        // render scene
        image.ForEach((x, y) =>
        {
            Ray ray = camera.CreateRay(new Vector2d(x, y));

            Color4? pixelColor = null;
            double nearest = double.PositiveInfinity;
            foreach (var shape in shapes)
            {
                var result = shape.Intersect(ray);
                if (result.Intersect)
                {
                    if (result.Distance < nearest)
                    {
                        nearest = result.Distance;
                        pixelColor = shape.Color;
                    }
                }
            }

            return pixelColor;
        });

        image.SavePFM(config.OutputFile);
        Logger.WriteLine("HDR image is finished.", LogType.Message);
    }
}