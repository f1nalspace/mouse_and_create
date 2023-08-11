using MouseAndCreate.Types;
using OpenTK.Mathematics;
using System;
using System.Diagnostics;

namespace MouseAndCreate.Graphics
{
    public class Image32 : IDisposable, IAssignable<Image32>, ICloneable<Image32>
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public Vector2i Size { get; private set; }
        public int Length => Size.X * Size.Y * 4;
        public byte[] Data { get; private set; }
        
        public Image32(Vector2i size = default, byte[] data = null, Guid? id = null, string name = null)
        {
            Id = id ?? Guid.NewGuid();
            Name = name;
            Size = size.X > 0 && size.Y > 0 ? size : Vector2i.Zero;
            Data = Length > 0 ? new byte[Length] : Array.Empty<byte>();
            if (data is not null)
            {
                if (data.Length != Data.Length)
                    throw new ArgumentException($"The image '{name}' data length is expected to be '{Data.Length}', but got '{data.Length}'. Please check that the size '{size}' does match the data!", nameof(data));
                Array.Copy(data, Data, data.Length);
            }
        }

        public void Crop(Vector2i newSize, Vector2i origin)
        {
            // TODO(final): Resize canvas only
        }

        public void Resize(Vector2i newSize)
        {
            // TODO(final): Resize of image with lacqos or something
        }

        public void Assign(Image32 other)
        {
            if (other is null)
                return;
            Name = other.Name;
            Size = other.Size;
            Data = new byte[other.Data.Length];
            Array.Copy(other.Data, Data, other.Data.Length);
        }

        public Image32 Clone()
        {
            Image32 result = new Image32();
            result.Assign(this);
            return result;
        }

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                Size = Vector2i.Zero;
                Data = null;
                _disposed = true;
            }
        }

         ~Image32()
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
