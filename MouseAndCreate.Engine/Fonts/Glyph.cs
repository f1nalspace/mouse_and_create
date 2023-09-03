using MouseAndCreate.Types;
using OpenTK.Mathematics;

namespace MouseAndCreate.Fonts;

public readonly struct Glyph
{
    public Rect4 Bounds { get; }
    public Rect4 UV { get; }
    public Vector3 Advance { get; }
    public int CodePoint { get; }

    public Glyph(int codePoint, Rect4 offset, Rect4 uv, Vector3 advance)
    {
        Bounds = offset;
        UV = uv;
        Advance = advance;
        CodePoint = codePoint;
    }

    public override string ToString() => $"[{CodePoint}] Offset: {Bounds}, UV: {UV}, Advance: {Advance}";
}
