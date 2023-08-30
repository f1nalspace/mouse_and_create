using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;

namespace MouseAndCreate.Rendering.OpenGL;

class OpenGLTexture : ITexture
{
    private readonly int _id;

    public string Name { get; }
    public int Width { get; }
    public int Height { get; }
    public TextureFormat Format { get; }

    public OpenGLTexture(string name, int width, int height, TextureFormat format, byte[] pixels)
    {
        Name = name;
        Width = width;
        Height = height;
        Format = format;

        _id = GL.GenTexture();

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

    protected virtual void Dispose(bool disposing)
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
