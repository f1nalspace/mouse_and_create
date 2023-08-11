using MouseAndCreate.Configurations;
using MouseAndCreate.Frames;
using MouseAndCreate.Input;
using MouseAndCreate.Play;
using MouseAndCreate.Rendering;
using OpenTK.Mathematics;
using System;

namespace MouseAndCreate.Editor
{
    public class GameEditor : Game
    {
        const float CameraZoomStep = 0.1f;
        const float CameraMoveStep = 10.0f;

        public GameEditor(GameSetup setup = null) : base(setup)
        {
            Camera.Offset = new Vector2(0, 0);
            Camera.Scale = new Vector2(1, 1);
            Camera.Zoom = 1.0f;
            Camera.Size = Setup.CameraSize;
        }

        public override void Resize(Vector2i newSize)
        {
            base.Resize(newSize);
            Camera.Size = newSize;
        }

        protected override void MouseWheel(Vector2 position, Vector2 offset)
        {
            int s = MathHelper.Sign(offset.Y);
            Camera.Zoom += s * CameraZoomStep;
        }

        protected override void KeyDown(Key key)
        {
            if (key == Key.Left)
            {
                Camera.Offset += new Vector2(-CameraMoveStep, 0);
            }
            else if (key == Key.Right)
            {
                Camera.Offset += new Vector2(CameraMoveStep, 0);
            }
            else if (key == Key.Up)
            {
                Camera.Offset += new Vector2(0, CameraMoveStep);
            }
            else if (key == Key.Down)
            {
                Camera.Offset += new Vector2(0, -CameraMoveStep);
            }
        }

        public override void Render(TimeSpan deltaTime)
        {
            IFrame frame = Frames.GetFrameById(ActiveFrameId);

            Vector2 totalSize = frame.Setup.TotalSize;

            Vector2 cameraSize = frame.Setup.CameraSize;

            Matrix4 vp = Camera.ViewProjection;

            _renderer.SetViewport(0, 0, WindowSize.X, WindowSize.Y);

            _renderer.Clear(Color4.LightGray);

            _renderer.DrawQuad(vp, new Vector2(0, 0), new Vector2(totalSize.X, totalSize.Y), Color4.White);
            _renderer.DrawRectangle(vp, new Vector2(0, 0), new Vector2(totalSize.X, totalSize.Y), 1.0f, Color4.Black);

            _renderer.DrawRectangle(vp, new Vector2(0, 0), new Vector2(cameraSize.X, cameraSize.Y), 1.0f, Color4.Blue);
        }
    }
}
