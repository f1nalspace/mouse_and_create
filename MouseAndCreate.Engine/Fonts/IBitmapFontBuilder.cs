using MouseAndCreate.Graphics;
using System;
using System.Collections.Generic;

namespace MouseAndCreate.Fonts;

public interface IBitmapFontBuilder
{
    IBitmapFontBuilderContext Begin(int width, int height, float spacing = 0);
    void Add(IBitmapFontBuilderContext context, string fontName, ReadOnlySpan<byte> fontData, int fontIndex, float fontSize, IEnumerable<CodePointRange> ranges);
    BitmapFont End(IBitmapFontBuilderContext context, ImageFlags flags);
}
