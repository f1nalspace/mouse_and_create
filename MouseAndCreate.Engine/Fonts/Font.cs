using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MouseAndCreate.Fonts;

public class Font : IFont
{
    // https://github.com/StbSharp/StbTrueTypeSharp/blob/master/samples/StbTrueTypeSharp.MonoGame.Test/FontBaker.cs

    private ImmutableDictionary<int, Glyph> _glyphs;

    public IReadOnlyDictionary<int, Glyph> Glyphs => _glyphs;
    public float FontSize { get; }
    public float LineAdvance { get; }
    public float Spacing { get; }

    internal Font(float fontSize, float lineAdvance, float spacing, IReadOnlyDictionary<int, Glyph> glyphs)
    {
        if (glyphs is null)
            throw new ArgumentNullException(nameof(glyphs));
        _glyphs = glyphs.ToImmutableDictionary();
        FontSize = fontSize;
        LineAdvance = lineAdvance;
        Spacing = spacing;
    }

    private bool _disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
                _glyphs = ImmutableDictionary<int, Glyph>.Empty;
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
