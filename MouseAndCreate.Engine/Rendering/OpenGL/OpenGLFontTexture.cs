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
    public float Spacing { get; }
    public float Ascent { get; }
    public float Descent { get; }

    public OpenGLFontTexture(Guid id, string name, int width, int height, TextureFormat format, byte[] pixels, IFont font) : base(id, name, width, height, format, pixels)
    {
        if (font is null)
            throw new ArgumentNullException(nameof(font));
        _glyphs = font.Glyphs.ToImmutableDictionary();
        FontSize = font.FontSize;
        LineAdvance = font.LineAdvance;
        Spacing = font.Spacing;
        Ascent = font.Ascent;
        Descent = font.Descent;
    }

    public override string ToString() => $"[FontTexture/{Id}] {Name}";

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
