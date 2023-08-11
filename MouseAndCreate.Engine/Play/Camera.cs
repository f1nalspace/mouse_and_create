using OpenTK.Mathematics;
using System;

namespace MouseAndCreate.Play
{
    public class Camera : ICamera
    {
        public Vector2 Size { get => _size; set => ChangeSize(value); }
        private Vector2 _size = Vector2.Zero;

        public Vector2 Offset { get => _offset; set => ChangeOffset(value); }
        private Vector2 _offset = Vector2.Zero;

        public Vector2 Scale { get => _scale; set => ChangeScale(value); }
        private Vector2 _scale = Vector2.One;

        public float Zoom { get => _zoom; set => ChangeZoom(value, _minZoom, _maxZoom); }
        private float _zoom = 1;

        public float MinZoom { get => _minZoom; set => ChangeZoom(_zoom, value, _maxZoom); }
        private float _minZoom = 0.1f;

        public float MaxZoom { get => _maxZoom; set => ChangeZoom(_zoom, _minZoom, value); }
        private float _maxZoom = 10;

        public Matrix4 Projection => _projection;
        private Matrix4 _projection = Matrix4.Identity;

        public Matrix4 View => _view;
        private Matrix4 _view = Matrix4.Identity;

        public Matrix4 ViewProjection => _viewProjection;
        private Matrix4 _viewProjection = Matrix4.Identity;

        public event CameraChanged Changed;

        public Camera(Vector2 size)
        {
            float sx = size.X * 0.5f;
            float sy = size.Y * 0.5f;
            _size = size;
            _view = Matrix4.Identity;
            _projection = Matrix4.CreateOrthographicOffCenter(-sx, sx, -sy, sy, -100.0f, 100.0f);
            _viewProjection = _view * _projection;
        }

        private void ChangeSize(Vector2 size)
        {
            if (!size.Equals(_size))
            {
                _size = size;
                float sx = size.X * 0.5f;
                float sy = size.Y * 0.5f;
                _projection = Matrix4.CreateOrthographicOffCenter(-sx, sx, -sy, sy, -100.0f, 100.0f);
                _viewProjection = _view * _projection;
                Changed?.Invoke(this);
            }
        }

        private void ChangeOffset(Vector2 offset)
        {
            if (!offset.Equals(_offset))
            {
                _offset = offset;
                UpdateView();
            }
        }

        private void ChangeScale(Vector2 scale)
        {
            if (!scale.Equals(_scale))
            {
                _scale = scale;
                UpdateView();
            }
        }

        private void ChangeZoom(float zoom, float min, float max)
        {
            if (!zoom.Equals(_zoom))
            {
                _zoom = Math.Max(min, Math.Min(max, zoom));
                UpdateView();
            }
        }

        private void UpdateView()
        {
            _view = Matrix4.CreateScale(_scale.X * _zoom, _scale.Y * _zoom, 1) * Matrix4.CreateTranslation(_offset.X, _offset.Y, 0);
            _viewProjection = _view * _projection;
            Changed?.Invoke(this);
        }

        public override string ToString() => FormattableString.Invariant($"D: ({Size.X} x {Size.Y}), T: ({Offset.X}, {Offset.Y}), S: ({Scale.X}, {Scale.Y}), Z: {Zoom}");
    }
}
