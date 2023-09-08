using MouseAndCreate.Graphics;
using System;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MouseAndCreate.Editor.Utils
{
    static class ImageUtils
    {
        public unsafe static ImageSource CreateImageSourceFrom(IImage image)
        {
            if (image is null)
                return null;

            int w = image.Width;
            int h = image.Height;

            int components = image.Components;

            PixelFormat format = image.Format switch
            {
                ImageFormat.Alpha => PixelFormats.Gray8,
                ImageFormat.RGB => PixelFormats.Bgr24,
                ImageFormat.RGBA => PixelFormats.Bgra32,
                _ => throw new NotSupportedException($"The image format '{image.Format}' is not supported")
            };

            int sourceStride = components * w;

            WriteableBitmap bitmap = new WriteableBitmap(w, h, 96, 96, format, null);

            nint targetBuffer = bitmap.BackBuffer;

            int targetStride = bitmap.BackBufferStride;

            byte* targetImagePtr = (byte*)targetBuffer.ToPointer();

            fixed (byte* sourceImagePtr = image.Data)
            {
                nint sourceScanline = new nint(sourceImagePtr);
                nint targetScanline = new nint(targetImagePtr);
                for (int y = 0; y < h; ++y)
                {
                    //Buffer.MemoryCopy(sourceScanline.ToPointer(), targetScanline.ToPointer(), targetStride, sourceStride);
                    for (int x = 0; x < w; ++x)
                    {
                        nint sourcePixel = sourceScanline + x * components;
                        nint targetPixel = targetScanline + x * components;
                        byte* sourcePtr = (byte*)sourcePixel.ToPointer();
                        byte* targetPtr = (byte*)targetPixel.ToPointer();
                        targetPtr[0] = sourcePtr[0];
                        targetPtr[1] = sourcePtr[1];
                        targetPtr[2] = sourcePtr[2];
                        targetPtr[3] = sourcePtr[3];
                    }
                    sourceScanline += sourceStride;
                    targetScanline += targetStride;
                }
            }

            bitmap.Lock();
            bitmap.AddDirtyRect(new System.Windows.Int32Rect(0, 0, w, h));
            bitmap.Unlock();

            bitmap.Freeze();

            return bitmap;
        }
    }
}
