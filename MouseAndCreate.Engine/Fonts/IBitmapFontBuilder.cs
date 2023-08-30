using System;
using System.Collections.Generic;
using System.IO;

namespace MouseAndCreate.Fonts;

public interface IBitmapFontBuilder
{
    IBitmapFontBuilderContext Begin(int width, int height);
    void Add(IBitmapFontBuilderContext context, string fontName, ReadOnlySpan<byte> fontData, int fontIndex, float fontSize, IEnumerable<CodePointRange> ranges);
    BitmapFont End(IBitmapFontBuilderContext context);
}
