﻿using MouseAndCreate.Graphics;
using OpenTK.Mathematics;
using System;

namespace MouseAndCreate.Rendering;

public static class IRendererExtensions
{
    public static ITexture LoadTexture(this IRenderer renderer, Guid id, ITextureSource source, TextureFormat target, ImageFlags flags)
    {
        TextureData data = source.Load(target, flags);
        return renderer.LoadTexture(id, source.Name, data);
    }

    public static void DrawQuad(this IRenderer renderer, Matrix4 viewProjection, Vector2 translation, Vector2 scale, Color4 color)
        => renderer.DrawQuad(viewProjection, new Vector3(translation), new Vector3(scale) { Z = 1.0f, }, color);
    public static void DrawQuad(this IRenderer renderer, Matrix4 viewProjection, float cx, float cy, float sx, float sy, Color4 color)
        => renderer.DrawQuad(viewProjection, new Vector3(cx, cy, 0), new Vector3(sx, sy, 1), color);

    public static void DrawQuad(this IRenderer renderer, Matrix4 viewProjection, Vector2 translation, Vector2 scale, ITexture texture, Color4? color = null)
        => renderer.DrawQuad(viewProjection, new Vector3(translation), new Vector3(scale) { Z = 1.0f, }, texture, color);
    public static void DrawQuad(this IRenderer renderer, Matrix4 viewProjection, float cx, float cy, float sx, float sy, ITexture texture, Color4? color = null)
        => renderer.DrawQuad(viewProjection, new Vector3(cx, cy, 0), new Vector3(sx, sy, 1), texture, color);

    public static void DrawLine(this IRenderer renderer, Matrix4 viewProjection, Vector2 p0, Vector2 p1, float thickness, Color4 color)
        => renderer.DrawLine(viewProjection, new Vector3(p0), new Vector3(p1), thickness, color);
    public static void DrawLine(this IRenderer renderer, Matrix4 viewProjection, float x0, float y0, float x1, float y1, float thickness, Color4 color)
        => renderer.DrawLine(viewProjection, new Vector3(x0, y0, 0), new Vector3(x1, y1, 0), thickness, color);

    public static void DrawRectangle(this IRenderer renderer, Matrix4 viewProjection, Vector2 translation, Vector2 scale, float thickness, Color4 color)
        => renderer.DrawRectangle(viewProjection, new Vector3(translation), new Vector3(scale), thickness, color);
    public static void DrawRectangle(this IRenderer renderer, Matrix4 viewProjection, float cx, float cy, float sx, float sy, float thickness, Color4 color)
        => renderer.DrawRectangle(viewProjection, new Vector3(cx, cy, 0), new Vector3(sx, sy, 1), thickness, color);

    public static void DrawString(this IRenderer renderer, Matrix4 viewProjection, Vector2 translation, string text, IFontTexture fontTexture, float scale = 1.0f, Color4? color = null)
        => renderer.DrawString(viewProjection, new Vector3(translation), text, fontTexture, scale, color);
}
