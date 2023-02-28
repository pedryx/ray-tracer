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
    }

    public void CreateSolids()
    {
        shapes.Add(new Sphere()
        {
            Position = new Vector3(0, 0, 10),
            Radius = 1,
            Color = Color4.Red,
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
            foreach (var shape in shapes)
            {
                if (shape.Intersect(ray) != null)
                {
                    pixelColor = shape.Color;
                    break;
                }
            }

            return pixelColor;
        });
    }
}