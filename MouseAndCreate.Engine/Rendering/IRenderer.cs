using MouseAndCreate.Graphics;
using OpenTK.Mathematics;
using System;
using System.IO;

namespace MouseAndCreate.Rendering
{
    public interface IRenderer : IDisposable
    {
        void Init();
        void Release();

        ITexture LoadTexture(string name, TextureData data);

        void SetViewport(int x, int y, int width, int height);
        void Clear(Color4 clearColor);
        void DrawQuad(Matrix4 viewProjection, Vector3 translation, Vector3 scale, Color4 color);
        void DrawQuad(Matrix4 viewProjection, Vector3 translation, Vector3 scale, ITexture texture, Color4? color = null, Vector4? uvAdjustment = null);
        void DrawLine(Matrix4 viewProjection, Vector3 p0, Vector3 p1, float thickness, Color4 color, LinePattern pattern = LinePattern.Solid, float stippleFactor = 2.0f);
        void DrawRectangle(Matrix4 viewProjection, Vector3 translation, Vector3 scale, float thickness, Color4 color);
        void CheckForErrors();
    }
}
