﻿using MouseAndCreate.Rendering;
using MouseAndCreate.Types;
using OpenTK.Mathematics;

namespace MouseAndCreate.Configurations
{
    public interface IGameSetup
    {
        Vector2i WindowSize { get; }
        Vector2 CameraSize { get; }
        Ratio Aspect { get; }
        string Title { get; }
        RendererType Renderer { get; }
        CoordinateSystem CoordinateSystem { get; }
        bool ShowCursor { get; }
        Color4 DefaultBackgroundColor { get; }
    }
}
