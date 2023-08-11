using OpenTK.Mathematics;
using StbTrueTypeSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MouseAndCreate.Fonts
{
    unsafe class BitmapFontCreator
    {
        private int _bitmapWidth, _bitmapHeight;
        private byte[] _data;
        private StbTrueType.stbtt_pack_context _context;
        private readonly Dictionary<int, GlyphInfo> _glyphs = new Dictionary<int, GlyphInfo>();

        public BitmapFontCreator()
        {
            _bitmapWidth = _bitmapHeight = 0;
            _data = Array.Empty<byte>();
            _context = null;
        }

        public void Begin(int width, int height)
        {
            _bitmapWidth = width;
            _bitmapHeight = height;
            _data = new byte[width * height];
            _context = new StbTrueType.stbtt_pack_context();
            _glyphs.Clear();

            fixed (byte* pixelsPtr = _data)
            {
                StbTrueType.stbtt_PackBegin(_context, pixelsPtr, width, height, width, 1, null);
            }
        }

        public void Add(string fontName, byte[] fontData, int fontIndex, float pixelHeight, IEnumerable<CodePointRange> ranges)
        {
            if (string.IsNullOrEmpty(fontName))
                throw new ArgumentNullException(nameof(fontName));
            if (fontData is null || fontData.Length == 0)
                throw new ArgumentNullException(nameof(fontData));
            if (fontIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(fontIndex), fontIndex, $"The font index '{fontIndex}' is out-of-range");
            if (pixelHeight <= 0)
                throw new ArgumentOutOfRangeException(nameof(pixelHeight), pixelHeight, $"The font pixel height '{pixelHeight}' is out-of-range");
            if (ranges is null)
                throw new ArgumentNullException(nameof(ranges));
            if (!ranges.Any())
                throw new ArgumentException("No code point ranges", nameof(ranges));

            int fontOffset;
            fixed (byte* fontPtr = fontData)
                fontOffset = StbTrueType.stbtt_GetFontOffsetForIndex(fontPtr, fontIndex);

            StbTrueType.stbtt_fontinfo fontInfo = StbTrueType.CreateFont(fontData, fontOffset);
            if (fontInfo is null)
                throw new InvalidDataException($"The font '{fontName}' with data length of '{fontData.Length}' failed to load");

            float scaleFactor = StbTrueType.stbtt_ScaleForPixelHeight(fontInfo, pixelHeight);

            int ascent, descent, lineGap;
            StbTrueType.stbtt_GetFontVMetrics(fontInfo, &ascent, &descent, &lineGap);

            foreach (CodePointRange range in ranges)
            {
                if (range.Start > range.End)
                    continue;

                StbTrueType.stbtt_packedchar[] cd = new StbTrueType.stbtt_packedchar[range.End - range.Start + 1];

                fixed (StbTrueType.stbtt_packedchar* chardataPtr = cd)
                {
                    StbTrueType.stbtt_PackFontRange(_context, fontInfo.data, 0, pixelHeight, range.Start, range.Length, chardataPtr);
                }

                for (int i = 0; i < cd.Length; ++i)
                {
                    float yOff = cd[i].yoff;
                    yOff += ascent * scaleFactor;

                    int codePoint = range.Start + i;

                    Vector4i rect = new Vector4i(cd[i].x0, cd[i].x0, cd[i].x1 - cd[i].x0, cd[i].y1 - cd[i].y0);

                    Vector2i offset = new Vector2i((int)cd[i].xoff, (int)Math.Round(yOff));

                    Vector2i advance = new Vector2i((int)Math.Round(cd[i].xadvance), 0);

                    GlyphInfo glyphInfo = new GlyphInfo(codePoint, rect, offset, advance);

                    _glyphs[codePoint] = glyphInfo;
                }
            }
        }

        public BitmapFont End()
        {
            StbTrueType.stbtt_PackEnd(_context);

            BitmapFont result = new BitmapFont();

            return result;
        }
    }
}
