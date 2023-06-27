using OpenTK.Mathematics;


namespace RayTracer.Shapes;
public class Plane : Shape
{
    private const double threshold = 1e-6;

    public Vector3d Normal;

    public override void Transform(Matrix4d transform)
    {
        Position = Vector3d.TransformPosition(Position, transform);
        Normal = Vector3d.TransformNormal(Normal, transform);
    }

    public override IntersectResult Intersect(Ray ray)
    {
        // t = (S - P_0) * N / (P_1 * N)
        double denominator = Vector3d.Dot(Normal, ray.Direction);

        // no intersection
        if (MathHelper.Abs(denominator) < threshold)
            return IntersectResult.False;

        double t = Vector3d.Dot(Position - ray.Position, Normal) / denominator;
            
        // point is in negative distance
        if (t < 0)
            return IntersectResult.False;
            
        return new IntersectResult(true, t, -1 * MathHelper.Sign(denominator) * Normal, this);
    }
}
