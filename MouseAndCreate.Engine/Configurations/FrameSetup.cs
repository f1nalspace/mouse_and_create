using MouseAndCreate.Configurations;
using MouseAndCreate.Types;
using OpenTK.Mathematics;

namespace MouseAndCreate.Configurations
{
    public class FrameSetup
    {
        public Vector2 TotalSize { get; set; } = GameSetup.DefaultCameraSize * 2;
        public Vector2 CameraSize { get; set; } = GameSetup.DefaultCameraSize;
        public Ratio Aspect { get; set; } = GameSetup.DefaultAspect;
        public string Name { get; set; }
        public Color4 BackgroundColor { get; set; } = new Color4(0.2f, 0.3f, 0.6f, 1.0f);

        public FrameSetup(Vector2 totalSize, Vector2 cameraSize, Ratio? aspect = null)
        {
            TotalSize = totalSize;
            CameraSize = cameraSize;
            Aspect = aspect ?? new Ratio(CameraSize.X, CameraSize.Y);
            Name = null;
        }

        public FrameSetup(IGameSetup gameSetup) : this(gameSetup.CameraSize * 2, gameSetup.CameraSize, gameSetup.Aspect)
        {
            BackgroundColor = gameSetup.DefaultBackgroundColor;
        }
    }
}
