using System.Collections.Generic;

namespace MouseAndCreate.Fonts
{
    public interface IFontBuilder
    {
        IFontBuilderContext Begin(int width, int height);
        void Add(IFontBuilderContext context, string fontName, byte[] fontData, int fontIndex, float fontSize, IEnumerable<CodePointRange> ranges);
        BitmapFont End(IFontBuilderContext context);
    }
}
