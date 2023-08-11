namespace MouseAndCreate.Buffers
{
    static class QuadBufferTexture
    {
        public const string Name = "QuadTexture";

        public const int Stride = 5 * sizeof(float);

        // Format: X,Y,Z,U,V
        public static readonly float[] Vertices = {
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // Bottom-left
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // Bottom-right
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // Top-right
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f, // Top-left
        };

        public static readonly uint[] Indices = {
            0, 1, 2, // BL, BR, TR
            2, 3, 0, // TR, TL, BL
        };
    }
}
