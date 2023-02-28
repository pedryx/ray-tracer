using OpenTK.Mathematics;


namespace RayTracer.Shapes;
class Sphere : Shape
{
    public Vector3d Position;
    public double Radius;

    public override Vector3d? Intersect(Ray ray)
    {
        // t^2 * (P_1 * P_1) + 2 * t * (P_0 * P_1) + (P_0 * P_0) - 1 = 0
        // ax^2+bx+c=0
        double a = Vector3d.Dot(ray.Position, ray.Position);
        double b = 2 * Vector3d.Dot(ray.Position, ray.Direction);
        double c = Vector3d.Dot(ray.Position, ray.Position) - 1;

        // D = b^2 - 4 ac
        double d = b * b - 4 * a * c;

        if (d > 0)
        {
            // two points intersection
            // t0, t1 = (-b +- sqrt(D)) / 2a
            double t0 = (-b + MathHelper.Sqrt(d)) / (2 * a);
            double t1 = (-b - MathHelper.Sqrt(d)) / (2 * a);

            // return nearest intersection point
            return ray.Position + MathHelper.Min(t0, t1) * ray.Direction;
        }
        else
        {
            // single point or no intersection
            return null;
        }
    }
}