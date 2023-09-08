using OpenTK.Mathematics;
using System;
using System.Collections.Immutable;

namespace MouseAndCreate.Graphics
{
    public static class ImageGenerator
    {
        public unsafe static Image32 CreateSolidImage(int width, int height, Color4 color)
        {
            byte[] data = new byte[width * height * 4];
            fixed (byte* p = data)
            {
                nint scanline = new nint(p);
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        nint cursor = scanline + x * 4;
                        byte* pixel = (byte*)cursor.ToPointer();
                        pixel[0] = (byte)(color.B * 255);
                        pixel[1] = (byte)(color.G * 255);
                        pixel[2] = (byte)(color.R * 255);
                        pixel[3] = (byte)(color.A * 255);
                    }
                    scanline += width * 4;
                }
            }
            Image32 result = new Image32(new Vector2i(width, height), data.ToImmutableArray(), Guid.NewGuid());
            return result;
        }
    }
}
