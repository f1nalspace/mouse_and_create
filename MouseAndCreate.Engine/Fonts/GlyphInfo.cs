using OpenTK.Mathematics;

namespace MouseAndCreate.Fonts;

public readonly struct GlyphInfo
{
    public Vector4 Rect { get; }
    public Vector2 Offset { get; }
    public Vector2 Advance { get; }
    public int CodePoint { get; }

    public GlyphInfo(int codePoint, Vector4 rect, Vector2 offset, Vector2 advance)
    {
        CodePoint = codePoint;
        Rect = rect;
        Offset = offset;
        Advance = advance;
    }
}
