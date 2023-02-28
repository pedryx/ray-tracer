using OpenTK.Mathematics;

using System;


// todo: camera from config file

namespace RayTracer;
/// <summary>
/// Represent simple perspective camera in 3D space.
/// </summary>
class Camera
{
    /// <summary>
    /// Position of the camera's eye.
    /// </summary>
    public Vector3d Position;
    /// <summary>
    /// Target towrds which is camera facing.
    /// </summary>
    public Vector3d Target;
    /// <summary>
    /// Up direction of the camera.
    /// </summary>
    public Vector3d Up;

    /// <summary>
    /// Field of view in y direction [radians].
    /// </summary>
    public double FOV;
    public Vector2d ViewPortSize;
    /// <summary>
    /// Distance of the near clip plane.
    /// </summary>
    public double NearPlane;
    /// <summary>
    /// Distance of the far clip plane.
    /// </summary>
    public double FarPlane;

    public Matrix4d GetView()
        => Matrix4d.LookAt(Position, Target, Up);

    public Matrix4d GetProjection()
        => Matrix4d.Perspective(FOV, ViewPortSize.X / ViewPortSize.Y, NearPlane, FarPlane);

    /// <summary>
    /// Create ray from camera's position towards specific viewport position.
    /// </summary>
    public Ray CreateRay(Vector2d position)
    {
        var direction = Position - Target;
        var planeMidpoint = Position + direction * NearPlane;
        var right = Vector3d.Cross(Up, direction);
        var shiftedPosition = (position - ViewPortSize / 2);
        var planePosition = planeMidpoint 
            + shiftedPosition.X * right + shiftedPosition.Y * Up + direction * NearPlane;
        
        return new Ray()
        {
            Position = Position,
            Direction = (planePosition - Position).Normalized(),
        };
    }
}