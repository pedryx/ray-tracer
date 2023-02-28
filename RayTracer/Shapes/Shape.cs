using OpenTK.Mathematics;


namespace RayTracer.Shapes;
abstract class Shape
{
    public Color4 Color = Color4.White;

    public abstract IntersectResult Intersect(Ray ray);

    public class IntersectResult
    {
        public bool Intersect { get; private set; }
        public Vector3d IntersectPoint { get; private set; }
        public double Distance { get; private set; }

        public IntersectResult(bool intersect, Vector3d intersectPoint, double distance)
        {
            Intersect = intersect;
            IntersectPoint = intersectPoint;
            Distance = distance;
        }
    }
}