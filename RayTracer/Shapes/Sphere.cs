using OpenTK.Mathematics;


namespace RayTracer.Shapes;
class Sphere : Shape
{
    public Vector3d Position;
    public double Radius;

    public override IntersectResult Intersect(Ray ray)
    {
        // t^2 * (P_1 * P_1) + 2 * t * ((P_0 - S) * P_1) + ((P_0 - S) * (P_0 - S)) - R^2 = 0
        // ax^2 + bx + c = 0
        double a = Vector3d.Dot(ray.Direction, ray.Direction);
        double b = 2 * Vector3d.Dot(ray.Position - Position, ray.Direction);
        double c = Vector3d.Dot(ray.Position - Position, ray.Position - Position) - Radius * Radius;

        // D = b^2 - 4 ac
        double d = b * b - 4 * a * c;

        if (d > 0)
        {
            // two points intersection
            // t0, t1 = (-b +- sqrt(D)) / 2a
            double t0 = (-b + MathHelper.Sqrt(d)) / (2 * a);
            double t1 = (-b - MathHelper.Sqrt(d)) / (2 * a);

            // points can be in negative directions
            if (t0 < 0 && t1 < 0)
                return new IntersectResult(false, Vector3d.Zero, double.PositiveInfinity);

            if (t0 < 0)
                return new IntersectResult(true, ray.Position + t1 * ray.Direction, t1);

            if (t1 < 0)
                return new IntersectResult(true, ray.Position + t0 * ray.Direction, t0);

            double nearest = MathHelper.Min(t0, t1);
            return new IntersectResult(true, ray.Position + nearest * ray.Direction, nearest);
        }
        else
        {
            // single point or no intersection
            return new IntersectResult(false, Vector3d.Zero, double.PositiveInfinity);
        }
    }
}