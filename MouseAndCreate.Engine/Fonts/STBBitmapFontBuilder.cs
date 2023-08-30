using MouseAndCreate.Graphics;
using OpenTK.Mathematics;
using StbTrueTypeSharp;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace MouseAndCreate.Fonts;

unsafe class STBBitmapFontBuilder : IBitmapFontBuilder
{
    class STBBitmapFontBuilderContext : IBitmapFontBuilderContext
    {
        public int Width { get; }
        public int Height { get; }

        public byte[] Data => _data;
        private readonly byte[] _data;

        internal readonly StbTrueType.stbtt_pack_context _context;

        public IReadOnlyDictionary<int, GlyphInfo> Glyphs => _glyphs;
        internal readonly Dictionary<int, GlyphInfo> _glyphs = new Dictionary<int, GlyphInfo>();

        public STBBitmapFontBuilderContext(int width, int height)
        {
            Width = width;
            Height = height;
            _data = new byte[width * height];
            _context = new StbTrueType.stbtt_pack_context();
        }
    }

    public IBitmapFontBuilderContext Begin(int width, int height)
    {
        STBBitmapFontBuilderContext context = new STBBitmapFontBuilderContext(width, height);
        fixed (byte* pixelsPtr = context.Data)
        {
            StbTrueType.stbtt_PackBegin(context._context, pixelsPtr, width, height, width, 1, null);
        }
        return context;
    }

    public void Add(IBitmapFontBuilderContext context, string fontName, ReadOnlySpan<byte> fontData, int fontIndex, float fontSize, IEnumerable<CodePointRange> ranges)
    {
        if (context is not STBBitmapFontBuilderContext correctContext)
            throw new ArgumentNullException(nameof(context));
        if (string.IsNullOrEmpty(fontName))
            throw new ArgumentNullException(nameof(fontName));
        if (fontData.Length == 0)
            throw new ArgumentException("Font data is empty", nameof(fontData));
        if (fontIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(fontIndex), fontIndex, $"The font index '{fontIndex}' is out-of-range. Please specify a value that is greater or equal than zero.");
        if (fontSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(fontSize), fontSize, $"The font size '{fontSize}' is out-of-range. Please specify a value that is greater than zero.");
        if (ranges is null)
            throw new ArgumentNullException(nameof(ranges));
        if (!ranges.Any())
            throw new ArgumentException("Code point ranges are empty. Please specify at least one code-point range.", nameof(ranges));

        using MemoryStream tmpStream = new MemoryStream(fontData.Length);
        byte[] font = tmpStream.GetBuffer();
        Span<byte> bufferSpan = font.AsSpan();
        fontData.CopyTo(bufferSpan);

        int fontOffset;
        fixed (byte* fontPtr = font)
            fontOffset = StbTrueType.stbtt_GetFontOffsetForIndex(fontPtr, fontIndex);

        StbTrueType.stbtt_fontinfo fontInfo = StbTrueType.CreateFont(font, fontOffset);
        if (fontInfo is null)
            throw new InvalidDataException($"The font '{fontName}' with data length of '{font.Length}' failed to load");

        float scaleFactor = StbTrueType.stbtt_ScaleForPixelHeight(fontInfo, fontSize);

        int ascent, descent, lineGap;
        StbTrueType.stbtt_GetFontVMetrics(fontInfo, &ascent, &descent, &lineGap);

        foreach (CodePointRange range in ranges)
        {
            if (range.Start > range.End)
                continue;

            StbTrueType.stbtt_packedchar[] cd = new StbTrueType.stbtt_packedchar[range.End - range.Start + 1];

            fixed (StbTrueType.stbtt_packedchar* chardataPtr = cd)
            {
                StbTrueType.stbtt_PackFontRange(correctContext._context, fontInfo.data, 0, fontSize, range.Start, range.Length, chardataPtr);
            }

            for (int i = 0; i < cd.Length; ++i)
            {
                float yOff = cd[i].yoff;
                yOff += ascent * scaleFactor;

                int codePoint = range.Start + i;

                Vector4i rect = new Vector4i(cd[i].x0, cd[i].y0, cd[i].x1 - cd[i].x0, cd[i].y1 - cd[i].y0);
                Vector2i offset = new Vector2i((int)cd[i].xoff, (int)Math.Round(yOff));
                Vector2i advance = new Vector2i((int)Math.Round(cd[i].xadvance), 0);

                GlyphInfo glyphInfo = new GlyphInfo(codePoint, rect, offset, advance);

                correctContext._glyphs[codePoint] = glyphInfo;
            }
        }
    }

    public BitmapFont End(IBitmapFontBuilderContext context)
    {
        if (context is not STBBitmapFontBuilderContext correctContext)
            throw new ArgumentNullException(nameof(context));
        StbTrueType.stbtt_PackEnd(correctContext._context);
        Image8 image = new Image8(correctContext.Width, correctContext.Height, correctContext.Data.ToImmutableArray());
        BitmapFont result = new BitmapFont(correctContext.Width, correctContext.Height, correctContext.Glyphs.ToImmutableDictionary(), image);
        return result;
    }
}
