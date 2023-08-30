using MouseAndCreate.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseAndCreate.Rendering
{
    public class ImageTextureSource : ITextureSource
    {
        public IImage Image { get; }
        public string Name { get; }

        public ImageTextureSource(IImage image, string name = null)
        {
            if (image is null)
                throw new ArgumentNullException(nameof(image));
            Image = image;
            Name = image.Name ?? name;
        }

        private static TextureData RGBToRGBA(IImage image)
        {
            byte[] data = ImageConverter.ConvertRGBToRGBA(image.Width, image.Height, image.Data);
            return new TextureData(image.Width, image.Height, data, TextureFormat.RGBA8);
        }

        private static TextureData AlphaToRGBA(IImage image, bool colorOnly)
        {
            byte[] data = ImageConverter.ConvertAlphaToRGBA(image.Width, image.Height, image.Data, colorOnly);
            return new TextureData(image.Width, image.Height, data, TextureFormat.RGBA8);
        }

        private static TextureData RGBAToAlpha(IImage image)
        {
            byte[] data = ImageConverter.ConvertRGBAToAlpha(image.Width, image.Height, image.Data);
            return new TextureData(image.Width, image.Height, data, TextureFormat.Alpha8);
        }

        public TextureData Load(TextureFormat target, ImageFlags flags)
        {
            switch (target)
            {
                case TextureFormat.RGBA8:
                {
                    return Image.Format switch
                    {
                        ImageFormat.RGBA => new TextureData(Image.Width, Image.Height, Image.Data, TextureFormat.RGBA8),
                        ImageFormat.RGB => RGBToRGBA(Image),
                        ImageFormat.Alpha => AlphaToRGBA(Image, false),
                        _ => throw new NotSupportedException($"The image format '{Image.Format}' is not supported to be converted into '{TextureFormat.RGBA8}'")
                    };
                };

                case TextureFormat.Alpha8:
                {
                    return Image.Format switch
                    {
                        ImageFormat.Alpha => new TextureData(Image.Width, Image.Height, Image.Data, TextureFormat.Alpha8),
                        ImageFormat.RGBA => RGBAToAlpha(Image),
                        _ => throw new NotSupportedException($"The image format '{Image.Format}' is not supported to be converted into '{TextureFormat.Alpha8}'")
                    };
                }

                default:
                    throw new NotSupportedException($"The target format '{target}' is not supported");
            }
        }
    }
}
