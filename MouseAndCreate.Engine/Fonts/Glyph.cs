using MouseAndCreate.Types;
using OpenTK.Mathematics;

namespace MouseAndCreate.Fonts;

public readonly struct Glyph
{
    public Rect4 Offset { get; }
    public Rect4 UV { get; }
    public Vector2 Advance { get; }
    public int CodePoint { get; }

    public Glyph(int codePoint, Rect4 offset, Rect4 uv, Vector2 advance)
    {
        Offset = offset;
        UV = uv;
        Advance = advance;
        CodePoint = codePoint;
    }

    public override string ToString() => $"[{CodePoint}] Offset: {Offset}, UV: {UV}, Advance: {Advance}";
}
