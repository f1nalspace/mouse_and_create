using OpenTK.Mathematics;

namespace MouseAndCreate.Fonts
{
    public readonly struct GlyphInfo
    {
        public int CodePoint { get; }
        public Vector4i Rect { get; }
        public Vector2i Offset { get; }
        public Vector2i Advance { get; }

        public GlyphInfo(int codePoint, Vector4i rect, Vector2i offset, Vector2i advance)
        {
            CodePoint = codePoint;
            Rect = rect;
            Offset = offset;
            Advance = advance;
        }
    }
}
