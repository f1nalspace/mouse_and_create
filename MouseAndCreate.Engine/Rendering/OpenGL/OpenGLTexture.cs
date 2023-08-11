using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using System;
using System.Diagnostics;
using System.IO;

namespace MouseAndCreate.Rendering.OpenGL
{
    class OpenGLTexture : ITexture
    {
        private readonly int _id;

        public string Name { get; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public OpenGLTexture(string name, int width = 0, int height = 0)
        {
            Name = name;
            Width = width;
            Height = height;
            _id = GL.GenTexture();
        }

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

        private void Upload(byte[] pixels, int width, int height, TextureFormat format)
        {
            PixelInternalFormat internalFormat = format switch
            {
                TextureFormat.RGBA8 => PixelInternalFormat.Rgba,
                TextureFormat.Alpha8 => PixelInternalFormat.Alpha,
                _ => throw new NotSupportedException($"The texture format '{format}' is not supported")
            };

            PixelFormat pixelFormat = format switch
            {
                TextureFormat.RGBA8 => PixelFormat.Rgba,
                TextureFormat.Alpha8 => PixelFormat.Alpha,
                _ => throw new NotSupportedException($"The texture format '{format}' is not supported")
            };

            GL.BindTexture(TextureTarget.Texture2D, _id);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, width, height, 0, pixelFormat, PixelType.UnsignedByte, pixels);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            Width = width;
            Height = height;
        }

        public void Upload(byte[] data, TextureFormat format, bool flipY = true)
        {
            ColorComponents components = GetRequiredComponents(format);
            StbImage.stbi_set_flip_vertically_on_load(flipY ? 1 : 0);
            ImageResult image = ImageResult.FromMemory(data, components);
            if (image is not null)
                Upload(image.Data, image.Width, image.Height, format);
        }

        public void Upload(Stream stream, TextureFormat format, bool flipY = true)
        {
            ColorComponents components = GetRequiredComponents(format);
            StbImage.stbi_set_flip_vertically_on_load(flipY ? 1 : 0);
            ImageResult image = ImageResult.FromStream(stream, components);
            if (image is not null)
                Upload(image.Data, image.Width, image.Height, format);
        }

        public void Bind(int index = 0)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + index);
            GL.BindTexture(TextureTarget.Texture2D, _id);
        }

        public void Unbind(int index = 0)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + index);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                GL.DeleteTexture(_id);
                _disposed = true;
            }
        }

        ~OpenGLTexture()
        {
            if (!_disposed)
                Debug.WriteLine($"[WARNING] Texture '{Name}' not disposed!");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
