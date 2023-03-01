using OpenTK.Mathematics;


namespace RayTracer;
abstract class LightSource
{
    public Vector3d Intensity;

    /// <summary>
    /// Compute reflectance color.
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
        => material.Color * material.Diffuse * Intensity;
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
        Vector3d lightDirection = Position - point;
        //Vector3d viewDirection = cameraPosition - point;

        return material.Color * material.Diffuse * Vector3d.Dot(lightDirection, normal) * Intensity;
    }
}
