using MouseAndCreate.Fonts;
using MouseAndCreate.Graphics;
using MouseAndCreate.Rendering;
using System.IO;

namespace MouseAndCreate.Play
{
    public interface IContentLoader
    {
        ITexture LoadTexture(string name, TextureData textureData);
        ITexture LoadTexture(ITextureSource source, TextureFormat format, ImageFlags flags);

        IFontTexture LoadFont(string name, IFont font, TextureData textureData);
        IFontTexture LoadFont(string name, Stream fontStream, float fontSize, CodePointRange[] ranges, ImageFlags imageFlags, int textureWidth = 512, int textureHeight = 512);
    }
}
