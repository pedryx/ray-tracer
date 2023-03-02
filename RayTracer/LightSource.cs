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
    /// <returns>Computed light source contribution.</returns>
    public abstract Vector3d Reflectance
    (
        Vector3d normal,
        Vector3d point,
        Vector3d cameraPosition,
        Material material
    );
}

class AmbientLightSource : LightSource
{
    /// <summary>
    /// Compute light source contribution to the color visible on the surface of a solid.
    /// </summary>
    /// <param name="point">Point where to compute the color.</param>
    /// <param name="cameraPosition">Position of camera.</param>
    /// <param name="material">Material of shape.</param>
    /// <returns>Computed light source contribution.</returns>
    public override Vector3d Reflectance
    (
        Vector3d normal,
        Vector3d point,
        Vector3d cameraPosition,
        Material material
    )
        => material.Ambient * Intensity;
}

class PointLightSource : LightSource
{
    public Vector3d Position;

    /// <summary>
    /// Compute light source contribution to the color visible on the surface of a solid.
    /// </summary>
    /// <param name="point">Point where to compute the color.</param>
    /// <param name="cameraPosition">Position of camera.</param>
    /// <param name="material">Material of shape.</param>
    /// <returns>Computed light source contribution.</returns>
    public override Vector3d Reflectance
    (
        Vector3d normal,
        Vector3d point,
        Vector3d cameraPosition,
        Material material
    )
    {
        Vector3d lightDirection = (Position - point).Normalized();
        Vector3d viewDirection = (cameraPosition - point).Normalized();
        double dotDiffuse = Vector3d.Dot(normal, lightDirection);
        Vector3d reflection = 2 * normal * dotDiffuse - lightDirection;
        double dotSpecular = Vector3d.Dot(reflection, viewDirection);

        Vector3d diffuse = dotDiffuse > 0 ? Intensity * material.Diffuse * dotDiffuse : Vector3d.Zero;
        Vector3d specular = Vector3d.Zero;
        if (dotSpecular > 0)
            specular = Intensity * material.Specular * MathHelper.Pow(dotSpecular, material.Highlight);

        return diffuse + specular;
    }
}