using MouseAndCreate.Fonts;
using MouseAndCreate.Graphics;
using OpenTK.Mathematics;
using System;

namespace MouseAndCreate.Rendering
{
    public interface IRenderer : IDisposable
    {
        void Init();
        void Release();

        ITexture LoadTexture(string name, TextureData textureData);
        ITexture LoadTexture(ITextureSource source, TextureFormat format, ImageFlags flags);

        IFontTexture LoadFont(string name, IFont font, TextureData textureData);

        void SetViewport(int x, int y, int width, int height);
        void Clear(Color4 clearColor);
        void DrawQuad(Matrix4 viewProjection, Vector3 translation, Vector3 scale, Color4 color);
        void DrawQuad(Matrix4 viewProjection, Vector3 translation, Vector3 scale, ITexture texture, Color4? color = null, Vector4? uvAdjustment = null);
        void DrawLine(Matrix4 viewProjection, Vector3 p0, Vector3 p1, float thickness, Color4 color, LinePattern pattern = LinePattern.Solid, float stippleFactor = 2.0f);
        void DrawRectangle(Matrix4 viewProjection, Vector3 translation, Vector3 scale, float thickness, Color4 color);
        void DrawString(Matrix4 viewProjection, Vector3 translation, string text, IFontTexture fontTexture, float scale = 1.0f, Color4? color = null);
        Vector2 MeasureString(string text, IFont fontTexture, float scale = 1.0f);
        void CheckForErrors();
    }
}
