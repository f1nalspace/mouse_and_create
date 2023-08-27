using System;

namespace MouseAndCreate.Rendering
{
    public readonly struct TextureData
    {
        public int Width { get; }
        public int Height { get; }
        public TextureFormat Format { get; }
        public byte[] Data { get; }

        public bool IsEmpty => Width <= 0 || Height <= 0 || Format == TextureFormat.None || Data is null || Data.Length == 0;

        public TextureData(int width, int height, byte[] data, TextureFormat format)
        {
            Width = width;
            Height = height;
            Data = data;
            Format = format;
        }

        public static readonly TextureData Empty = new TextureData(0,0, Array.Empty<byte>(), TextureFormat.None);
    }
}
