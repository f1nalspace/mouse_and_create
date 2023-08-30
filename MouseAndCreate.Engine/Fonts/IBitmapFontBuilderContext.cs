using System.Collections.Generic;

namespace MouseAndCreate.Fonts;

public interface IBitmapFontBuilderContext
{
    int Width { get; }
    int Height { get; }
    byte[] Data { get; }
    IReadOnlyDictionary<int, GlyphInfo> Glyphs { get; }
}
