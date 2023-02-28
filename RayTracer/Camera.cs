using OpenTK.Mathematics;


// todo: camera from config file

namespace RayTracer;
/// <summary>
/// Represent simple perspective camera in 3D space.
/// </summary>
class Camera
{
    private Vector2d viewport;
    private Vector2d resolution;
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

    public Camera(Vector2d resolution)
    {
        this.resolution = resolution;
        aspectRation = resolution.X / resolution.Y;
        FOV = 40;
    }

    public Camera(double x, double y)
        : this(new Vector2d(x, y)) { }

    /// <summary>
    /// Create ray from camera's position towards specific viewport position.
    /// </summary>
    public Ray CreateRay(Vector2d position)
    {
        var viewportPosition = position / (resolution - Vector2d.One) * viewport - viewport / 2;
        var planeMidpoint = Position + Direction * NearPlane;
        var right = Vector3d.Cross(Up, Direction);
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