namespace MouseAndCreate.Buffers
{
    static class QuadBufferColor
    {
        public const string Name = "QuadColor";

        public const int Stride = 3 * sizeof(float);

        // Format: X,Y,Z
        public static readonly float[] Vertices = {
            -0.5f, -0.5f, 0.0f, // Bottom-left
             0.5f, -0.5f, 0.0f, // Bottom-right
             0.5f,  0.5f, 0.0f, // Top-right
            -0.5f,  0.5f, 0.0f, // Top-left
        };

        public static readonly uint[] Indices = {
            0, 1, 2, // BL, BR, TR
            2, 3, 0, // TR, TL, BL
        };
    }
}
