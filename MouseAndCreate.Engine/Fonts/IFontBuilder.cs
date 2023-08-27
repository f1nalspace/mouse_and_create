using System;
using System.Collections.Generic;
using System.IO;

namespace MouseAndCreate.Fonts
{
    public interface IFontBuilder
    {
        IFontBuilderContext Begin(int width, int height);
        void Add(IFontBuilderContext context, string fontName, ReadOnlySpan<byte> fontData, int fontIndex, float fontSize, IEnumerable<CodePointRange> ranges);
        BitmapFont End(IFontBuilderContext context);
    }
}
