using MouseAndCreate.Graphics;
using System;
using System.IO;

namespace MouseAndCreate.Rendering
{
    public interface ITextureLoader
    {
        TextureData Load(ReadOnlySpan<byte> data, TextureFormat format, ImageFlags flags);
        TextureData Load(Stream stream, TextureFormat format, ImageFlags flags);
    }
}
