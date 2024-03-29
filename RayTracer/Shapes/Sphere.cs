﻿using OpenTK.Mathematics;

namespace RayTracer.Shapes;
public sealed class Sphere : Shape
{
    public double Radius;

    public override void Transform(Matrix4d transform)
    {
        Vector3d point = Position + Vector3d.UnitX * Radius;
        Position = Vector3d.TransformPosition(Position, transform);
        point = Vector3d.TransformPosition(point, transform);
        Radius = Vector3d.Distance(point, Position);
    }

    public override IntersectResult Intersect(Ray ray)
    {
        // t^2 * (P_1 * P_1) + 2 * t * ((P_0 - S) * P_1) + ((P_0 - S) * (P_0 - S)) - R^2 = 0
        // ax^2 + bx + c = 0
        double a = Vector3d.Dot(ray.Direction, ray.Direction);
        double b = 2 * Vector3d.Dot(ray.Position - Position, ray.Direction);
        double c = Vector3d.Dot(ray.Position - Position, ray.Position - Position) - Radius * Radius;

        // D = b^2 - 4 ac
        double d = b * b - 4 * a * c;

        // single point or no intersection
        if (d <= 0)
            return IntersectResult.False;

        // t0, t1 = (-b +- sqrt(D)) / 2a
        double t0 = (-b + MathHelper.Sqrt(d)) / (2 * a);
        double t1 = (-b - MathHelper.Sqrt(d)) / (2 * a);

        // points can be in negative distance
        if (t0 < 0 && t1 < 0)
            return IntersectResult.False;

        double distance;
        if (t0 < 0)
            distance = t0;
        else if (t1 < 0)
            distance = t1;
        else
            distance = MathHelper.Min(t0, t1);

        Vector3d normal = (ray.At(distance) - Position).Normalized();
        bool frontFace = Vector3d.Dot(ray.Direction, normal) < 0.0;
        if (!frontFace)
            normal *= -1;

        return new IntersectResult(true, distance, normal, this, frontFace);
    }
}