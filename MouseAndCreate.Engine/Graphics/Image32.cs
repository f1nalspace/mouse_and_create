using MouseAndCreate.Types;
using OpenTK.Mathematics;
using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace MouseAndCreate.Graphics
{
    public class Image32 : IAssignable<Image32>, ICloneable<Image32>, IImage
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public Vector2i Size { get; private set; }
        public int Length => Size.X * Size.Y * 4;

        public int Width => Size.X;
        public int Height => Size.Y;
        public ImageFormat Format => ImageFormat.RGBA;
        public ReadOnlySpan<byte> Data => _data.AsSpan();
        private ImmutableArray<byte> _data;

        public Image32(Vector2i size, ImmutableArray<byte> data, Guid? id = null, string name = null)
        {
            int expectedLen = size.X * size.Y * 4;
            if (data.Length != expectedLen)
                throw new ArgumentException($"The image '{name}' data length is expected to be '{expectedLen}', but got '{data.Length}'. Please check that the size '{size}' does match the data!", nameof(data));
            Id = id ?? Guid.NewGuid();
            Name = name;
            Size = size;
            _data = data;
        }

        public unsafe static Image32 FromAlpha(Vector2i size, ReadOnlySpan<byte> alpha, Guid? id = null, string name = null)
        {
            if (size.X <= 0 || size.Y <= 0)
                throw new ArgumentOutOfRangeException($"The size must be greater than zero", size, nameof(size));
            int expectedLength = size.X * size.Y;
            if (alpha.Length != expectedLength)
                throw new ArgumentException($"The image '{name}' data length is expected to be '{expectedLength}', but got '{alpha.Length}'. Please check that the size '{size}' does match the alpha!", nameof(alpha));
            byte[] data = new byte[size.X * 4 * size.Y];
            fixed (byte* baseTarget = data)
            {
                IntPtr destScanLine = new IntPtr(baseTarget);
                fixed (byte* baseSource = alpha)
                {
                    IntPtr sourceScanLine = new IntPtr(baseSource);
                    for (int y = 0; y < size.Y; ++y)
                    {
                        for (int x = 0; x < size.X; ++x)
                        {
                            IntPtr src = IntPtr.Add(sourceScanLine, x);
                            IntPtr dst = IntPtr.Add(destScanLine, x * 4);
                            byte* sourcePixel = (byte*)src.ToPointer();
                            byte* destPixel = (byte*)dst.ToPointer();
                            byte a = *sourcePixel;
                            destPixel[0] = a;
                            destPixel[1] = a;
                            destPixel[2] = a;
                            destPixel[3] = a;
                        }
                        sourceScanLine = IntPtr.Add(sourceScanLine, size.X);
                        destScanLine = IntPtr.Add(destScanLine, size.X * 4);
                    }
                }
            }
            return new Image32(size, data.ToImmutableArray(), id, name);
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
            _data = other.Data.ToImmutableArray();
        }

        public Image32 Clone()
        {
            Image32 result = new Image32(Vector2i.Zero, ImmutableArray<byte>.Empty, Id, Name);
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
