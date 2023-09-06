using MouseAndCreate.Graphics;
using MouseAndCreate.Types;
using OpenTK.Mathematics;
using StbTrueTypeSharp;
using System;
using System.Collections.Generic;
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
        public float MaxDescent { get; set; }
        public float MaxAscent { get; set; }
        public float Spacing { get; }

        internal readonly StbTrueType.stbtt_pack_context _context;

        public IReadOnlyDictionary<int, Glyph> Glyphs => _glyphs;
        internal readonly Dictionary<int, Glyph> _glyphs = new Dictionary<int, Glyph>();

        public STBBitmapFontBuilderContext(int width, int height, float spacing)
        {
            Width = width;
            Height = height;
            Spacing = spacing;
            MaxFontSize = 0;
            MaxLineGap = 0;
            MaxAscent = 0;
            MaxDescent = 0;
            _data = new byte[width * height];
            _context = new StbTrueType.stbtt_pack_context();
        }
    }

    public IBitmapFontBuilderContext Begin(int width, int height, float spacing = 0)
    {
        STBBitmapFontBuilderContext context = new STBBitmapFontBuilderContext(width, height, spacing);
        fixed (byte* pixelsPtr = context.Data)
        {
            StbTrueType.stbtt_PackBegin(context._context, pixelsPtr, width, height, width, 1, null);
            StbTrueType.stbtt_PackSetOversampling(context._context, 2, 2);
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
        correctContext.MaxAscent = Math.Max(correctContext.MaxAscent, ascent * scaleFactor);
        correctContext.MaxDescent = Math.Max(correctContext.MaxDescent, descent * scaleFactor);

        float invWidth = 1.0f / (float)correctContext.Width;
        float invHeight = 1.0f / (float)correctContext.Height;
        Vector2 texel = new Vector2(invWidth, invHeight);

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
                float x0 = cd[i].xoff;
                float x1 = cd[i].xoff2;
                float y0 = cd[i].yoff;
                float y1 = cd[i].yoff2;

                int codePoint = range.Start + i;

                Rect4 rect = new Rect4(cd[i].x0, cd[i].y0, cd[i].x1 - cd[i].x0, cd[i].y1 - cd[i].y0);
                Rect4 uv = new Rect4(rect.Offset.X * texel.X, rect.Offset.Y * texel.Y, rect.Size.X * texel.X, rect.Size.Y * texel.Y);
                Rect4 offset = new Rect4(x0, y0, x1 - x0, y1 - y0);
                Vector3 advance = new Vector3(0, rect.Width, cd[i].xadvance - rect.Width);

                Glyph glyph = new Glyph(codePoint, offset, uv, advance);

                correctContext._glyphs[codePoint] = glyph;
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

        Dictionary<int, Glyph> newGlyphs = new Dictionary<int, Glyph>();

        // Get smallest Y offset
        float minOffsetY = float.MaxValue;
        foreach (KeyValuePair<int, Glyph> glyphPair in context.Glyphs)
        {
            Glyph glyph = glyphPair.Value;
            if (glyph.Bounds.Y < minOffsetY)
                minOffsetY = glyph.Bounds.Y;
        }

        foreach (KeyValuePair<int, Glyph> glyphPair in context.Glyphs)
        {
            int key = glyphPair.Key;
            Glyph oldGlyph = glyphPair.Value;

            Vector2 uSize = oldGlyph.UV.Size;
            Vector2 uOffset = oldGlyph.UV.Offset;
            Rect4 uv;
            if (flags.HasFlag(ImageFlags.FlipY))
                uv = new Rect4(uOffset.X, (1.0f - uOffset.Y - uSize.Y), uSize.X, uSize.Y);
            else
                uv = oldGlyph.UV;

            Vector2 rsize = oldGlyph.Bounds.Size;

            Vector2 roffset;
            if (flags.HasFlag(ImageFlags.FlipY))
                roffset = oldGlyph.Bounds.Offset * new Vector2(1, -1) + new Vector2(0, -rsize.Y);
            else
                roffset = oldGlyph.Bounds.Offset;

            Glyph newGlyph = new Glyph(oldGlyph.CodePoint, new Rect4(roffset, rsize), uv, oldGlyph.Advance);
            newGlyphs.Add(key, newGlyph);
        }

        BitmapFont result = new BitmapFont(
            id: Guid.NewGuid(),
            fontSize: correctContext.MaxFontSize,
            lineAdvance: correctContext.MaxLineGap,
            spacing: correctContext.Spacing,
            ascent: correctContext.MaxAscent,
            descent: correctContext.MaxDescent,
            glyphs: newGlyphs,
            image: image);
        return result;
    }
}
