using MouseAndCreate.Graphics;
using StbImageSharp;
using System;
using System.IO;

namespace MouseAndCreate.Rendering
{
    class STBTextureLoader : ITextureLoader
    {
        static ColorComponents GetRequiredComponents(TextureFormat format)
        {
            ColorComponents result = format switch
            {
                TextureFormat.RGBA8 => ColorComponents.RedGreenBlueAlpha,
                TextureFormat.Alpha8 => ColorComponents.Grey,
                _ => throw new NotSupportedException($"The texture format '{format}' is not supported")
            };
            return result;
        }

        public TextureData Load(ReadOnlySpan<byte> data, TextureFormat format, TextureLoadFlags flags)
        {
            byte[] mem = data.ToArray();
            ColorComponents components = GetRequiredComponents(format);
            StbImage.stbi_set_flip_vertically_on_load(flags.HasFlag(TextureLoadFlags.FlipY) ? 1 : 0);
            ImageResult image = ImageResult.FromMemory(mem, components);
            if (image is null)
                return TextureData.Empty;
            return new TextureData(image.Width, image.Height, image.Data, format);
        }

        public TextureData Load(Stream stream, TextureFormat format, TextureLoadFlags flags)
        {
            ColorComponents components = GetRequiredComponents(format);
            StbImage.stbi_set_flip_vertically_on_load(flags.HasFlag(TextureLoadFlags.FlipY) ? 1 : 0);
            ImageResult image = ImageResult.FromStream(stream, components);
            if (image is null)
                return TextureData.Empty;
            return new TextureData(image.Width, image.Height, image.Data, format);
        }
    }
}
