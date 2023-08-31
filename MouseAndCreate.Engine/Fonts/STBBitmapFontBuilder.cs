using MouseAndCreate.Graphics;
using MouseAndCreate.Types;
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

        public float MaxFontSize { get; set; }
        public float MaxLineGap { get; set; }

        internal readonly StbTrueType.stbtt_pack_context _context;

        public IReadOnlyDictionary<int, GlyphInfo> Glyphs => _glyphs;
        internal readonly Dictionary<int, GlyphInfo> _glyphs = new Dictionary<int, GlyphInfo>();

        public STBBitmapFontBuilderContext(int width, int height)
        {
            Width = width;
            Height = height;
            MaxFontSize = 0;
            MaxLineGap = 0;
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

        if (lineGap == 0)
            lineGap = ascent - descent;

        correctContext.MaxFontSize = Math.Max(correctContext.MaxFontSize, fontSize);
        correctContext.MaxLineGap = Math.Max(correctContext.MaxLineGap, lineGap * scaleFactor);

        float invWidth = 1.0f / (float)correctContext.Width;
        float invHeight = 1.0f / (float)correctContext.Width;
        Vector2 invSize = new Vector2(invWidth, invHeight);

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

                Rect4 rect = new Rect4(cd[i].x0, cd[i].y0, cd[i].x1 - cd[i].x0, cd[i].y1 - cd[i].y0);
                Rect4 uv = new Rect4(rect.Offset * invSize, rect.Size * invSize);
                Vector2 offset = new Vector2((int)cd[i].xoff, (int)Math.Round(yOff));
                Vector2 advance = new Vector2((int)Math.Round(cd[i].xadvance), 0);

                GlyphInfo glyphInfo = new GlyphInfo(codePoint, rect, uv, offset, advance);

                correctContext._glyphs[codePoint] = glyphInfo;
            }
        }
    }

    public BitmapFont End(IBitmapFontBuilderContext context, ImageFlags flags)
    {
        if (context is not STBBitmapFontBuilderContext correctContext)
            throw new ArgumentNullException(nameof(context));
        StbTrueType.stbtt_PackEnd(correctContext._context);

        if (flags.HasFlag(ImageFlags.FlipY))
            ImageConverter.FlipY(correctContext.Data, correctContext.Width, correctContext.Height, 1);

        Image8 image = new Image8(correctContext.Width, correctContext.Height, correctContext.Data);

        BitmapFont result = new BitmapFont(correctContext.MaxFontSize, correctContext.MaxLineGap, correctContext.Glyphs, image);
        return result;
    }
}
