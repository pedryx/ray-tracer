using OpenTK.Mathematics;


namespace RayTracer;
/// <summary>
/// Represent simple perspective camera in 3D space.
/// </summary>
public class Camera
{
    /// <summary>
    /// Size of projection plane.
    /// </summary>
    private Vector2d viewport;
    /// <summary>
    /// Size of image.
    /// </summary>
    private Vector2d resolution;
    /// <summary>
    /// Field of view.
    /// </summary>
    private double fov;
    private double aspectRation;

    /// <summary>
    /// Position of the camera's eye.
    /// </summary>
    public Vector3d Position;
    /// <summary>
    /// Target towrds which is camera facing.
    /// </summary>
    public Vector3d Direction = Vector3d.UnitZ;
    /// <summary>
    /// Up direction of the camera.
    /// </summary>
    public Vector3d Up = Vector3d.UnitY;
    /// <summary>
    /// Distance of the near clip plane.
    /// </summary>
    public double NearPlane = 1;
    /// <summary>
    /// Vertical field of view in degrees.
    /// </summary>
    public double FOV
    {
        get => fov;
        set
        {
            fov = value;
            viewport.Y = 2 * MathHelper.Tan(MathHelper.DegreesToRadians(fov) / 2);
            viewport.X = aspectRation * viewport.Y;
        }
    }
    /// <summary>
    /// Size of image.
    /// </summary>
    public Vector2d Resolution
    {
        get => resolution;
        set
        {
            resolution = value;
            aspectRation = resolution.X / resolution.Y;
        }
    }

    /// <summary>
    /// Create new camera for image of size zero.
    /// </summary>
    public Camera() {}

    /// <summary>
    /// Create new camera for image with specific resolution.
    /// </summary>
    /// <param name="resolution">Size of image.</param>
    public Camera(Vector2d resolution)
    {
        Resolution = resolution;
        FOV = 40;
    }

    /// <summary>
    /// Create new camera for image with specific width and height.
    /// </summary>
    public Camera(double width, double height)
        : this(new Vector2d(width, height)) { }

    /// <summary>
    /// Create ray from camera's position towards specific viewport position.
    /// </summary>
    public Ray CreateRay(Vector2d position)
    {
        // local position on projection plane
        var viewportPosition = position / (resolution - Vector2d.One) * viewport - viewport / 2;
        // concreate position of the middle of projection plane
        var planeMidpoint = Position + Direction * NearPlane;
        // camera's right direction
        var right = Vector3d.Cross(Up, Direction);
        // concreate position of the pixel
        var onPlanePosition = planeMidpoint
            + viewportPosition.X * right - viewportPosition.Y * Up;
        
        return new Ray()
        {
            Position = Position,
            Direction = (onPlanePosition - Position).Normalized(),
        };
    }

    public Ray CreateRay(double x, double y)
        => CreateRay(new Vector2d(x, y));
}