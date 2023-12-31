﻿using MouseAndCreate.Rendering;
using MouseAndCreate.Types;
using OpenTK.Mathematics;

namespace MouseAndCreate.Configurations;

public class GameSetup : IAssignable<GameSetup>, IGameSetup
{
    public const int DefaultWindowWidth = 640;
    public const int DefaultWindowHeight = 480;

    public const int DefaultCameraWidth = DefaultWindowWidth / 2;
    public const int DefaultCameraHeight = DefaultWindowHeight / 2;

    public static readonly Vector2i DefaultWindowSize = new Vector2i(DefaultWindowWidth, DefaultWindowHeight);

    public static readonly Vector2 DefaultCameraSize = new Vector2(DefaultCameraWidth, DefaultCameraHeight);

    public static readonly Ratio DefaultAspect = new Ratio(4.0f, 3.0f);

    public static readonly Color4 DefaultBackground = new Color4(0.2f, 0.3f, 0.6f, 1.0f);

    public const CoordinateSystem DefaultCoordinateSystem = CoordinateSystem.Cartesian;

    public Vector2i WindowSize { get; set; } = DefaultWindowSize;
    public Vector2 CameraSize { get; set; } = DefaultCameraSize;
    public Color4 BackgroundColor { get; set; } = DefaultBackground;
    public Ratio Aspect { get; set; } = DefaultAspect;
    public string Title { get; set; } = "Game";
    public RendererType Renderer { get; set; } = RendererType.OpenGL;
    public CoordinateSystem CoordinateSystem { get; set; } = DefaultCoordinateSystem;
    public bool ShowCursor { get; set; } = true;

    public GameSetup(Vector2i windowSize, Ratio? aspect = null, string title = "Game", CoordinateSystem cordinateSystem = DefaultCoordinateSystem)
    {
        WindowSize = windowSize;
        Aspect = aspect ?? new Ratio(WindowSize.X, WindowSize.Y);
        Title = title;
        CoordinateSystem = cordinateSystem;
    }

    public GameSetup(int width, int height, Ratio? aspect = null, string title = "Game", CoordinateSystem cordinateSystem = DefaultCoordinateSystem) : 
        this(new Vector2i(width, height), aspect, title, cordinateSystem)
    {
    }

    public static readonly GameSetup Default = new GameSetup(1280, 720, new Ratio(16, 9));

    public void Assign(GameSetup other)
    {
        if (other is null)
            return;
        WindowSize = other.WindowSize;
        CameraSize = other.CameraSize;
        BackgroundColor = other.BackgroundColor;
        Aspect = other.Aspect;
        Title = other.Title;
        Renderer = other.Renderer;
        CoordinateSystem = other.CoordinateSystem;
        ShowCursor = other.ShowCursor;
    }
}
