﻿using OpenTK.Mathematics;

using RayTracer.Shapes;

namespace RayTracer;
/// <summary>
/// Re[resent a result of intersection between shape and ray.
/// </summary>
public class IntersectResult
{
    /// <summary>
    /// Represent negative intersection result.
    /// </summary>
    public readonly static IntersectResult False = new(
        false,
        double.PositiveInfinity,
        Vector3d.Zero,
        null,
        false
    );

    /// <summary>
    /// Determine if ray intersected with shape.
    /// </summary>
    public bool Intersect { get; private init; }
    /// <summary>
    /// Distance from ray position to the intersection point.
    /// </summary>
    public double Distance { get; private init; }
    /// <summary>
    /// Normal at intersection point.
    /// </summary>
    public Vector3d Normal { get; private init; }

    public string Material { get; private init; }
    public Shape Shape { get; private init; }
    public bool FrontFace { get; private init; }

    /// <summary>
    /// Create <see cref="IntersectResult"/> from another intersection result and add information about
    /// material of intersected shape.
    /// </summary>
    /// <param name="material">Material of intersected shape.</param>
    public IntersectResult(IntersectResult result, string material)
    {
        Intersect = result.Intersect;
        Distance = result.Distance;
        Normal = result.Normal;
        Shape = result.Shape;
        FrontFace = result.FrontFace;
        Material = material;
    }

    /// <summary>
    /// Create <see cref="IntersectResult"/> as a result from intersection between ray and shape.
    /// </summary>
    /// <param name="intersect">Determine if ray intersected with shape.</param>
    /// <param name="distance">Distance from ray position to the intersection point.</param>
    /// <param name="normal">Normal at intersection point.</param>
    public IntersectResult(bool intersect, double distance, Vector3d normal, Shape shape, bool frontFace)
    {
        Intersect = intersect;
        Distance = distance;
        Normal = normal;
        Shape = shape;
        FrontFace = frontFace;
    }
}
