using System;
using System.IO;

namespace MouseAndCreate.Rendering
{
    public class TextureLoaderFactory : ITextureLoaderFactory
    {
        private readonly ITextureLoader[] _loaders;

        public static ITextureLoaderFactory Instance { get { return _instance ??= new TextureLoaderFactory(); } }
        private static ITextureLoaderFactory _instance = null;

        private TextureLoaderFactory()
        {
            _loaders = new ITextureLoader[] { 
                new STBTextureLoader()
            };
        }

        public TextureData Load(ReadOnlySpan<byte> data, TextureFormat format, TextureLoadFlags flags)
        {
            foreach (ITextureLoader loader in _loaders)
            {
                TextureData res = loader.Load(data, format, flags);
                if (!res.IsEmpty)
                    return res;
            }
            return TextureData.Empty;
        }

        public TextureData Load(Stream stream, TextureFormat format, TextureLoadFlags flags)
        {
            foreach (ITextureLoader loader in _loaders)
            {
                TextureData res = loader.Load(stream, format, flags);
                if (!res.IsEmpty)
                    return res;
            }
            return TextureData.Empty;
        }
    }
}
