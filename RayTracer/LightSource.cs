using OpenTK.Mathematics;


namespace RayTracer;
abstract class LightSource
{
    public Vector3d Intensity;

    /// <summary>
    /// Compute light source contribution to the color visible on the surface of a solid.
    /// </summary>
    /// <param name="point">Point where to compute the color.</param>
    /// <param name="cameraPosition">Position of camera.</param>
    /// <param name="material">Material of shape.</param>
    /// <returns>Computed color.</returns>
    public abstract Vector3d Reflectance
    (
        Vector3d normal,
        Vector3d point,
        //Vector3d cameraPosition,
        Material material
    );
}

class AmbientLightSource : LightSource
{
    public override Vector3d Reflectance(Vector3d normal, Vector3d point, Material material)
        => material.Ambient * Intensity;
}

class PointLightSource : LightSource
{
    public Vector3d Position;

    public override Vector3d Reflectance
    (
        Vector3d normal,
        Vector3d point,
        //Vector3d cameraPosition,
        Material material
    )
    {
        Vector3d lightDirection = (Position - point).Normalized();
        //Vector3d viewDirection = cameraPosition - point;

        var dot = Vector3d.Dot(lightDirection, normal);
        if (dot > 0 )
            return material.Diffuse * dot * Intensity;
        else
            return Vector3d.Zero;
    }
}

class DirectionalLightSource : LightSource
{
    public Vector3d Direction;

    public override Vector3d Reflectance
    (
        Vector3d normal,
        Vector3d point,
        Material material
    )
    {
        var dot = Vector3d.Dot(-1 * Direction, normal);
        if (dot > 0)
            return material.Diffuse * dot * Intensity;
        else
            return Vector3d.Zero;
    }
}
