using MouseAndCreate.Types;
using OpenTK.Mathematics;

namespace MouseAndCreate.Fonts;

public readonly struct GlyphInfo
{
    public Rect4 Rect { get; }
    public Rect4 UV { get; }
    public Vector2 Offset { get; }
    public Vector2 Advance { get; }
    public int CodePoint { get; }

    public GlyphInfo(int codePoint, Rect4 rect, Rect4 uv, Vector2 offset, Vector2 advance)
    {
        Rect = rect;
        UV = uv;
        Offset = offset;
        Advance = advance;
        CodePoint = codePoint;
    }

    public override string ToString() => $"[{CodePoint}] {Rect}";
}
