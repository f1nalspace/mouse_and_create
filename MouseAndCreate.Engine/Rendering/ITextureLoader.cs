using System;
using System.IO;

namespace MouseAndCreate.Rendering
{
    public interface ITextureLoader
    {
        TextureData Load(ReadOnlySpan<byte> data, TextureFormat format, TextureLoadFlags flags);
        TextureData Load(Stream stream, TextureFormat format, TextureLoadFlags flags);
    }
}
