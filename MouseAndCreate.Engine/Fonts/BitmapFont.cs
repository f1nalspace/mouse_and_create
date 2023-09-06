using MouseAndCreate.Graphics;
using System;
using System.Collections.Generic;

namespace MouseAndCreate.Fonts;

public class BitmapFont : Font
{
    public Image8 Image { get; }

    internal BitmapFont(Guid id, float fontSize, float lineAdvance, float spacing, float ascent, float descent, IReadOnlyDictionary<int, Glyph> glyphs, Image8 image) : base(id, fontSize, lineAdvance, spacing, ascent, descent, glyphs)
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
