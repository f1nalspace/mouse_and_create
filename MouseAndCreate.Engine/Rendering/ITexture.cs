using System;
using System.IO;

namespace MouseAndCreate.Rendering
{
    public interface ITexture : IDisposable
    {
        string Name { get; }
        int Width { get; }
        int Height { get; }

        void Upload(byte[] data, TextureFormat format, bool flipY = true);
        void Upload(Stream stream, TextureFormat format, bool flipY = true);

        void Bind(int index = 0);
        void Unbind(int index = 0);
    }
}
