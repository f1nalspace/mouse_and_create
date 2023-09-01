using MouseAndCreate.Fonts;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MouseAndCreate.Rendering.OpenGL;

class OpenGLFontTexture : OpenGLTexture, IFontTexture
{
    private ImmutableDictionary<int, Glyph> _glyphs;

    public IReadOnlyDictionary<int, Glyph> Glyphs => _glyphs;
    public float FontSize { get; }
    public float LineAdvance { get; }

    public OpenGLFontTexture(string name, int width, int height, TextureFormat format, byte[] pixels, IFont font) : base(name, width, height, format, pixels)
    {
        if (font is null)
            throw new ArgumentNullException(nameof(font));
        _glyphs = font.Glyphs.ToImmutableDictionary();
        FontSize = font.FontSize;
        LineAdvance = font.LineAdvance;
    }

    private bool _disposed = false;

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
                _glyphs = ImmutableDictionary<int, Glyph>.Empty;
            _disposed = true;
        }
        base.Dispose(disposing);
    }
}
