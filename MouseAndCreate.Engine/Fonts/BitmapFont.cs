using MouseAndCreate.Graphics;
using System.Collections.Immutable;

namespace MouseAndCreate.Fonts
{
    public class BitmapFont
    {
        // https://github.com/StbSharp/StbTrueTypeSharp/blob/master/samples/StbTrueTypeSharp.MonoGame.Test/FontBaker.cs

        public ImmutableDictionary<int, GlyphInfo> Glyphs { get; }
        public ImmutableArray<byte> Pixels { get; }
        public int Width { get; }
        public int Height { get; }

        internal BitmapFont(int width, int height, ImmutableDictionary<int, GlyphInfo> glyphs, ImmutableArray<byte> pixels)
        {
            Width = width;
            Height = height;
            Glyphs = glyphs;
            Pixels = pixels;
        }
    }
}
