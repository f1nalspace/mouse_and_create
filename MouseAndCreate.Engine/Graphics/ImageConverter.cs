using System;
using System.Runtime.InteropServices;

namespace MouseAndCreate.Graphics
{
    public static class ImageConverter
    {
        public unsafe static byte[] ConvertRGBAToAlpha(int width, int height, ReadOnlySpan<byte> rgba)
        {
            byte[] result = new byte[width * height];
            fixed (byte* baseTarget = result)
            {
                IntPtr destScanLine = new IntPtr(baseTarget);
                fixed (byte* baseSource = rgba)
                {
                    IntPtr sourceScanLine = new IntPtr(baseSource);
                    for (int y = 0; y < height; ++y)
                    {
                        for (int x = 0; x < width; ++x)
                        {
                            IntPtr src = IntPtr.Add(sourceScanLine, x * 4);
                            IntPtr dst = IntPtr.Add(destScanLine, x);
                            byte* sourcePixel = (byte*)src.ToPointer();
                            byte* destPixel = (byte*)dst.ToPointer();
                            byte a = sourcePixel[0];
                            *destPixel = a;
                        }
                        sourceScanLine = IntPtr.Add(sourceScanLine, width * 4);
                        destScanLine = IntPtr.Add(destScanLine, width);
                    }
                }
            }
            return result;
        }

        public unsafe static byte[] ConvertAlphaToRGBA(int width, int height, ReadOnlySpan<byte> alpha, bool colorOnly)
        {
            byte[] result = new byte[width * height * 4];
            fixed (byte* baseTarget = result)
            {
                IntPtr destScanLine = new IntPtr(baseTarget);
                fixed (byte* baseSource = alpha)
                {
                    IntPtr sourceScanLine = new IntPtr(baseSource);
                    for (int y = 0; y < height; ++y)
                    {
                        for (int x = 0; x < width; ++x)
                        {
                            IntPtr src = IntPtr.Add(sourceScanLine, x);
                            IntPtr dst = IntPtr.Add(destScanLine, x * 4);
                            byte* sourcePixel = (byte*)src.ToPointer();
                            byte* destPixel = (byte*)dst.ToPointer();
                            byte a = *sourcePixel;
                            destPixel[0] = a;
                            destPixel[1] = a;
                            destPixel[2] = a;
                            destPixel[3] = colorOnly ? (byte)255 : a;
                        }
                        sourceScanLine = IntPtr.Add(sourceScanLine, width);
                        destScanLine = IntPtr.Add(destScanLine, width * 4);
                    }
                }
            }
            return result;
        }

        public unsafe static byte[] ConvertRGBToRGBA(int width, int height, ReadOnlySpan<byte> rgb)
        {
            byte[] result = new byte[width * height * 4];
            fixed (byte* baseTarget = result)
            {
                IntPtr destScanLine = new IntPtr(baseTarget);
                fixed (byte* baseSource = rgb)
                {
                    IntPtr sourceScanLine = new IntPtr(baseSource);
                    for (int y = 0; y < height; ++y)
                    {
                        for (int x = 0; x < width; ++x)
                        {
                            IntPtr src = IntPtr.Add(sourceScanLine, x);
                            IntPtr dst = IntPtr.Add(destScanLine, x * 4);
                            byte* sourcePixel = (byte*)src.ToPointer();
                            byte* destPixel = (byte*)dst.ToPointer();
                            byte b = sourcePixel[0];
                            byte g = sourcePixel[1];
                            byte r = sourcePixel[2];
                            destPixel[0] = 255;
                            destPixel[1] = b;
                            destPixel[2] = g;
                            destPixel[3] = r;
                        }
                        sourceScanLine = IntPtr.Add(sourceScanLine, width * 3);
                        destScanLine = IntPtr.Add(destScanLine, width * 4);
                    }
                }
            }
            return result;
        }

        public unsafe static void FlipY(Span<byte> pixels, int width, int height, int components)
        {
            int scanline = width * components;
            byte[] tmpBuffer = new byte[scanline];
            fixed (byte* pixelBytePtr = pixels)
            {
                fixed (byte* tmpBytePtr = tmpBuffer)
                {
                    IntPtr tmpPtr = new IntPtr(tmpBytePtr);
                    IntPtr lowPtr = new IntPtr(pixelBytePtr);
                    IntPtr highPtr = IntPtr.Add(lowPtr, (height - 1) * scanline);
                    for (; lowPtr < highPtr; lowPtr += scanline, highPtr -= scanline)
                    {
                        Buffer.MemoryCopy(lowPtr.ToPointer(), tmpPtr.ToPointer(), scanline, scanline);
                        Buffer.MemoryCopy(highPtr.ToPointer(), lowPtr.ToPointer(), scanline, scanline);
                        Buffer.MemoryCopy(tmpPtr.ToPointer(), highPtr.ToPointer(), scanline, scanline);
                    }
                }
            }
        }
    }
}
