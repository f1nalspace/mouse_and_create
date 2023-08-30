using MouseAndCreate.Graphics;
using System;
using System.Collections.Immutable;

namespace MouseAndCreate.Fonts;

public class BitmapFont : Font
{
    public Image8 Image { get; }

    internal BitmapFont(float fontSize, float lineAdvance, ImmutableDictionary<int, GlyphInfo> glyphs, Image8 image) : base(fontSize, lineAdvance, glyphs)
    {
        Image = image ?? throw new ArgumentNullException(nameof(image));
    }

    private bool _disposed = false;

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
                Image.Dispose();
            _disposed = true;
        }
        base.Dispose(disposing);
    }
}
