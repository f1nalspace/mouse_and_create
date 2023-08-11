using OpenTK.Mathematics;
using System;
using System.IO;

namespace MouseAndCreate.Rendering
{
    public interface IRenderer : IDisposable
    {
        void Init();
        void Release();

        

        ITexture LoadTexture(string name, byte[] data, TextureFormat format, bool flipY = true);
        ITexture LoadTexture(string name, Stream stream, TextureFormat format, bool flipY = true);
        ITexture LoadTexture(ITextureSource source, TextureFormat format, bool flipY = true);

        void SetViewport(int x, int y, int width, int height);
        void Clear(Color4 clearColor);
        void DrawQuad(Matrix4 viewProjection, Vector3 translation, Vector3 scale, Color4 color);
        void DrawQuad(Matrix4 viewProjection, Vector3 translation, Vector3 scale, ITexture texture, Color4? color = null, Vector4? uvAdjustment = null);
        void DrawLine(Matrix4 viewProjection, Vector3 p0, Vector3 p1, float thickness, Color4 color);
        void DrawRectangle(Matrix4 viewProjection, Vector3 translation, Vector3 scale, float thickness, Color4 color);
        void CheckForErrors();
    }
}
