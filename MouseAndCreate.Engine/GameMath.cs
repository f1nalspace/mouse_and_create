using MouseAndCreate.Types;
using OpenTK.Mathematics;
using System;

namespace MouseAndCreate
{
    public static class GameMath
    {
        public static Vector2 Unproject(Vector2 screenPos, Matrix4 modelMat, Matrix4 projectionMat, Vector4 viewport, Vector2i screenSize)
        {
            // Based on GLM::gtc::unProjectZO()

            Matrix4 inverse = Matrix4.Invert(projectionMat * modelMat);

            float x = screenPos.X;
            float y = screenSize.Y - screenPos.Y; // Correct for OpenGL coordinate system being Y positive

            Vector4 tmp = new Vector4(x, y, 0, 1);

            tmp.X = (tmp.X - viewport[0]) / viewport[2];
            tmp.Y = (tmp.Y - viewport[1]) / viewport[3];

            tmp.X = tmp.X * 2.0f - 1.0f;
            tmp.Y = tmp.Y * 2.0f - 1.0f;

            Vector4 obj = inverse * tmp;
            obj /= obj.W;

            Vector2 result = new Vector2(obj.X, obj.Y);

            return result;
        }

        public static Vector2 Project(Vector2 worldPos, Matrix4 modelMat, Matrix4 projectionMat, Vector4 viewport, Vector2i screenSize)
        {
            // Based on GLM::gtc::projectZO()

            Vector4 tmp = new Vector4(worldPos.X, worldPos.Y, 0, 1);
            tmp = modelMat * tmp;
            tmp = projectionMat * tmp;

            tmp /= tmp.W;

            tmp.X = tmp.X * 0.5f + 0.5f;
            tmp.Y = tmp.Y * 0.5f + 0.5f;

            tmp[0] = tmp[0] * viewport[2] + viewport[0];
            tmp[1] = tmp[1] * viewport[3] + viewport[1];

            tmp[1] = screenSize.Y - tmp[1]; // Correct for OpenGL coordinate system being Y positive

            Vector2 result = new Vector2(tmp.X, tmp.Y);

            return result;
        }

        public static Viewport ComputeViewport(Vector2i currentSize, Vector2i initialSize, Ratio frameAspect)
        {
            int targetWidth = (int)(currentSize.X / frameAspect.Value);

            int x;
            int y;
            int w;
            int h;
            float scale;
            if (targetWidth > currentSize.Y)
            {
                h = currentSize.Y;
                w = (int)(currentSize.Y * frameAspect.Value);
                x = (currentSize.X - w) / 2;
                y = 0;
                scale = Math.Max(0.5f, currentSize.Y / (float)initialSize.Y);
            }
            else
            {
                w = currentSize.X;
                h = (int)(currentSize.X / frameAspect.Value);
                x = 0;
                y = (currentSize.Y - h) / 2;
                scale = Math.Max(0.5f, currentSize.X / (float)initialSize.X);
            }

            return new Viewport(x, y, w, h, scale);
        }
    }
}
