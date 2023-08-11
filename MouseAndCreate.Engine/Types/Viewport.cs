namespace MouseAndCreate.Types
{
    public readonly struct Viewport
    {
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }
        public float Scale { get; }

        public Viewport(int x, int y, int width, int height, float scale)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Scale = scale;
        }
    }
}
