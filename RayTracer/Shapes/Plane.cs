using OpenTK.Mathematics;

using System.Net.Mail;

namespace RayTracer.Shapes;
class Plane : Shape
{
    private const double threshold = 1e-6;

    public Vector3d Normal;
    public Vector3d Position;

    public override IntersectResult Intersect(Ray ray)
    {
        // P * N + d = 0

        // t = - (P_0 * N + d) / (V * N)
        // t = (S - P_0) * N / (P_1 * N)
        double denominator = Vector3d.Dot(Normal, ray.Direction);

        // no intersection
        if (MathHelper.Abs(denominator) < threshold)
            return IntersectResult.False;

        double t = Vector3d.Dot(Position - ray.Position, Normal) / denominator;
            
        // point is in negative distance
        if (t < 0)
            return IntersectResult.False;
            
        return new IntersectResult(true, t, MathHelper.Sign(denominator) * Normal, Material);
    }
}
