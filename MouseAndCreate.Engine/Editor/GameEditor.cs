using MouseAndCreate.Configurations;
using MouseAndCreate.Frames;
using MouseAndCreate.Input;
using MouseAndCreate.Platform;
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

        private CursorType _lastMouseMoveCursor = CursorType.Arrow;
        private bool _mouseMoveActive = false;
        private Vector2 _mouseMoveStart = Vector2.Zero;

        private bool _spaceDown = false;

        public GameEditor(IWindowManager windowMng, IInputQuery inputQuery, GameSetup setup = null) : base(windowMng, inputQuery, setup)
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

        protected override void MouseButtonDown(Vector2 position, MouseButton button)
        {
            if (button == MouseButton.Left && _spaceDown && !_mouseMoveActive)
            {
                _lastMouseMoveCursor = _windowMng.GetCursor();
                _windowMng.SetCursor(CursorType.Move);
                _mouseMoveActive = true;
                _mouseMoveStart = CurrentMousePos;
            }
        }

        protected override void MouseButtonUp(Vector2 position, MouseButton button)
        {
            if (button == MouseButton.Left && _mouseMoveActive)
            {
                _mouseMoveActive = false;
                _mouseMoveStart = Vector2.Zero;
                _windowMng.SetCursor(_lastMouseMoveCursor);
            }
        }

        protected override void KeyDown(Key key, KeyModifiers modifiers, bool isRepeat)
        {
            switch (key)
            {
                case Key.Left:
                    Camera.Offset += new Vector2(-CameraMoveStep, 0);
                    break;

                case Key.Right:
                    Camera.Offset += new Vector2(CameraMoveStep, 0);
                    break;

                case Key.Up:
                    Camera.Offset += new Vector2(0, CameraMoveStep);
                    break;

                case Key.Down:
                    Camera.Offset += new Vector2(0, -CameraMoveStep);
                    break;

                case Key.Space:
                    _spaceDown = true;
                    break;
            }
        }

        protected override void KeyUp(Key key, KeyModifiers modifiers, bool wasRepeat)
        {
            switch (key)
            {
                case Key.Space:
                    _spaceDown = false;
                    break;
            }
        }

        public override void Update(TimeSpan deltaTime)
        {
            if (_mouseMoveActive)
            {
                Vector2 deltaMove = CurrentMousePos - _mouseMoveStart;
                if (deltaMove.LengthSquared > 0)
                {
                    Camera.Offset += new Vector2(deltaMove.X, -deltaMove.Y);
                }
                _mouseMoveStart = CurrentMousePos;
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
