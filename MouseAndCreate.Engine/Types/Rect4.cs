using OpenTK.Mathematics;

namespace MouseAndCreate.Types
{
    public readonly struct Rect4
    {
        public Vector2 Offset { get; }
        public Vector2 Size { get; }

        public float X => Offset.X;
        public float Y => Offset.Y;
        public float Width => Size.X;
        public float Height => Size.Y;

        public Rect4(Vector2 offset, Vector2 size)
        {
            Offset = offset;
            Size = size;
        }

        public Rect4(float x, float y, float width, float height) : this(new Vector2(x, y), new Vector2(width, height))
        {
        }

        public Vector4 ToVec4() => new Vector4(Offset.X, Offset.Y, Size.X, Size.Y);
    }
}
