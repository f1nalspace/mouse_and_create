using MouseAndCreate.Types;
using OpenTK.Mathematics;
using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace MouseAndCreate.Graphics
{
    public class Image8 : IAssignable<Image8>, ICloneable<Image8>, IImage
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public Vector2i Size { get; private set; }
        public int Length => Size.X * Size.Y;

        public int Width => Size.X;
        public int Height => Size.Y;
        public ImageFormat Format => ImageFormat.RGBA;
        public ReadOnlySpan<byte> Data => _data.AsSpan();
        private ImmutableArray<byte> _data;

        public Image8(int width, int height, ImmutableArray<byte> data, Guid? id = null, string name = null)
        {
            int expectedLen = width * height;
            if (data.Length != expectedLen)
                throw new ArgumentException($"The image '{name}' data length is expected to be '{expectedLen}', but got '{data.Length}'. Please check that the size '{width} x {height}' does match the data!", nameof(data));
            Id = id ?? Guid.NewGuid();
            Name = name;
            Size = new Vector2i(width, height);
            _data = data;
        }

        public void Crop(Vector2i newSize, Vector2i origin)
        {
            // TODO(final): Resize canvas only
        }

        public void Resize(Vector2i newSize)
        {
            // TODO(final): Resize of image with lacqos or something
        }

        public void Assign(Image8 other)
        {
            if (other is null)
                return;
            Name = other.Name;
            Size = other.Size;
            _data = other._data;
        }

        public Image8 Clone()
        {
            Image8 result = new Image8(0, 0, ImmutableArray<byte>.Empty, Id, Name);
            result.Assign(this);
            return result;
        }

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                Size = Vector2i.Zero;
                _data = ImmutableArray<byte>.Empty;
                _disposed = true;
            }
        }

        ~Image8()
        {
            if (!_disposed)
                Debug.WriteLine($"[WARNING] Image '{Name}' not disposed!");
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


    }
}
