using System;
using System.Collections.Generic;
using System.IO;

namespace MouseAndCreate.Fonts;

public static class FontBuilderExtensions
{
    public static void Add(this IBitmapFontBuilder builder, IBitmapFontBuilderContext context, string fontName, Stream fontStream, int fontIndex, float fontSize, IEnumerable<CodePointRange> ranges)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));
        if (context is null)
            throw new ArgumentNullException(nameof(context));
        if (fontStream is null)
            throw new ArgumentNullException(nameof(fontStream));
        using MemoryStream tmpStream = new MemoryStream();
        fontStream.CopyTo(tmpStream);
        byte[] buffer = tmpStream.GetBuffer();
        builder.Add(context, fontName, buffer, fontIndex, fontSize, ranges);
    }
}
