using OpenTK.Mathematics;

namespace MouseAndCreate.Play
{
    public delegate void CameraChanged(ICamera camera);

    public interface ICamera
    {
        Vector2 Size { get; set; }
        Vector2 Offset { get; set; }
        Vector2 Scale { get; set; }
        float Zoom { get; set; }
        float MinZoom { get; set; }
        float MaxZoom { get; set; }

        Matrix4 Projection { get; }
        Matrix4 View { get; }
        Matrix4 ViewProjection { get; }

        event CameraChanged Changed;
    }
}
