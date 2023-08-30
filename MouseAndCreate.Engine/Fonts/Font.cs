using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MouseAndCreate.Fonts;

public class Font : IFont
{
    // https://github.com/StbSharp/StbTrueTypeSharp/blob/master/samples/StbTrueTypeSharp.MonoGame.Test/FontBaker.cs

    private ImmutableDictionary<int, GlyphInfo> _glyphs;

    public IReadOnlyDictionary<int, GlyphInfo> Glyphs => _glyphs;
    public float FontSize { get; }
    public float LineAdvance { get; }

    internal Font(float fontSize, float lineAdvance, ImmutableDictionary<int, GlyphInfo> glyphs)
    {
        if (glyphs is null)
            throw new ArgumentNullException(nameof(glyphs));
        _glyphs = glyphs;
        FontSize = fontSize;
        LineAdvance = lineAdvance;
    }

    private bool _disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
                _glyphs = ImmutableDictionary<int, GlyphInfo>.Empty;
            _disposed = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        System.GC.SuppressFinalize(this);
    }
}
