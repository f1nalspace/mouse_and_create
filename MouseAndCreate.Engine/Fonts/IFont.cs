using System;
using System.Collections.Generic;

namespace MouseAndCreate.Fonts;

public interface IFont : IDisposable
{
    IReadOnlyDictionary<int, GlyphInfo> Glyphs { get; }
    float FontSize { get; }
    float LineAdvance { get; }
}
