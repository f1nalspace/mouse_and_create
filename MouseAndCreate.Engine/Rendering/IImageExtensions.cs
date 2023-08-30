using MouseAndCreate.Graphics;
using System;

namespace MouseAndCreate.Rendering
{
    static class IImageExtensions
    {
        public static TextureData ToTextureData(this IImage image)
        {
            if (image is Image8 image8)
                return new TextureData(image.Width, image.Height, image.Data, TextureFormat.Alpha8);
            else if (image is Image32 image32)
                return new TextureData(image32.Width, image32.Height, image32.Data, TextureFormat.RGBA8);
            else
                throw new NotSupportedException($"The image '{image}' is not supported");
        }
    }
}
