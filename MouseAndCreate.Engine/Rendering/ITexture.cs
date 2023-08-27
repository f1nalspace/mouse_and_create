using System;

namespace MouseAndCreate.Rendering
{
    public interface ITexture : IDisposable
    {
        string Name { get; }
        int Width { get; }
        int Height { get; }
        TextureFormat Format { get; }

        void Bind(int index = 0);
        void Unbind(int index = 0);
    }
}
