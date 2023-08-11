﻿using MouseAndCreate.Configurations;
using MouseAndCreate.Frames;
using MouseAndCreate.Input;
using MouseAndCreate.Objects;
using MouseAndCreate.Rendering;
using MouseAndCreate.Textures;
using MouseAndCreate.Types;
using OpenTK.Mathematics;
using System;
using System.Diagnostics;

namespace MouseAndCreate.Play;

public class Game : IGame, IGameInputManager
{
    private readonly GameSetup _setup;
    private readonly IGameObjectManager _gameObjectManager;
    private readonly IFrameManager _frameManager;
    private readonly ICamera _camera;

    protected readonly IRenderer _renderer;

    public GameSetup Setup => _setup;
    public IGameObjectManager Objects => _gameObjectManager;
    public IFrameManager Frames => _frameManager;
    public ICamera Camera => _camera;

    public Guid ActiveFrameId { get => _activeFrameId; set => ChangeFrameById(value); }
    private Guid _activeFrameId = Guid.Empty;
    public event ActiveFrameChangedEventHandler ActiveFrameChanged;
    private IFrame _activeFrame = null;
    private readonly object _activeFrameLock = new object();

    public Vector2i WindowSize { get; private set; } = Vector2i.Zero;
    public Vector2 CurrentMousePos { get; set; } = new Vector2i(-1000, -1000);
    public bool IsMouseInside { get; set; } = true;

    protected ITexture _mouseArrowTexture = null;
    protected ITexture _testTexture = null;

    public Game(GameSetup setup = null)
    {
        _setup = (setup ??= GameSetup.Default);

        IRendererFactory rendererFactory = new RendererFactory();
        _renderer = rendererFactory.Create(setup.Renderer);

        _gameObjectManager = new GameObjectManager();
        _frameManager = new FrameManager(this);
        _camera = new Camera(setup.CameraSize);

        _renderer.Init();

        _mouseArrowTexture = _renderer.LoadTexture(DefaultTextures.MouseArrow, TextureFormat.RGBA8);
        _testTexture = _renderer.LoadTexture(TestTextures.OpenGLTestTexture, TextureFormat.RGBA8);
    }

    private void ChangeFrameById(Guid id)
    {
        if (!_activeFrameId.Equals(id))
        {
            lock (_activeFrameLock)
            {
                _activeFrameId = id;
                _activeFrame = _frameManager.GetFrameById(id);
            }
            ActiveFrameChanged?.Invoke(this, _activeFrame);
        }
    }

    #region Input
    void IMouseInputManager.MouseEnter()
    {
        IsMouseInside = true;
    }

    void IMouseInputManager.MouseLeave()
    {
        IsMouseInside = false;
    }

    void IMouseInputManager.MouseMove(Vector2 position)
    {
        CurrentMousePos = position;
    }

    protected virtual void MouseButtonDown(Vector2 position, MouseButton button) { }
    void IMouseInputManager.MouseButtonDown(Vector2 position, MouseButton button)
    {
        Debug.WriteLine($"MouseButton pressed: {button}");
        MouseButtonDown(position, button);
    }

    protected virtual void MouseButtonUp(Vector2 position, MouseButton button) { }
    void IMouseInputManager.MouseButtonUp(Vector2 position, MouseButton button)
    {
        Debug.WriteLine($"MouseButton released: {button}");
        MouseButtonUp(position, button);
    }

    protected virtual void MouseWheel(Vector2 position, Vector2 offset) { }
    void IMouseInputManager.MouseWheel(Vector2 position, Vector2 offset)
    {
        Debug.WriteLine($"MouseWheel changed: {offset}");
        MouseWheel(position, offset);
    }

    protected virtual void KeyDown(Key key) { }
    void IKeyboardInputManager.KeyDown(Key key)
    {
        if (key != Key.None)
        {
            Debug.WriteLine($"Key down: {key}");
            KeyDown(key);
        }
    }

    protected virtual void KeyUp(Key key) { }
    void IKeyboardInputManager.KeyUp(Key key)
    {
        if (key != Key.None)
        {
            Debug.WriteLine($"Key up: {key}");
            KeyUp(key);
        }
    }

    void IKeyboardInputManager.Input(string input)
    {
        if (!string.IsNullOrEmpty(input))
            Debug.WriteLine($"Input: {input}");
    }
    #endregion

    public virtual void Resize(Vector2i newSize)
    {
        WindowSize = newSize;
    }

    public virtual void Update(TimeSpan deltaTime)
    {

    }

    public virtual void Render(TimeSpan deltaTime)
    {
        IFrame frame = null;
        if (!Guid.Empty.Equals(ActiveFrameId))
        {
            frame = Frames.GetFrameById(ActiveFrameId);
        }

        if (frame is null)
        {
            _renderer.SetViewport(0, 0, WindowSize.X, WindowSize.Y);
            _renderer.Clear(Color4.Black);
            return;
        }

        Vector2i initialSize = Setup.WindowSize;

        Vector2i winSize = WindowSize;

        Ratio frameAspect = frame.Setup.Aspect;

        Viewport viewport = GameMath.ComputeViewport(winSize, initialSize, frameAspect);

        float lineScale = viewport.Scale;

        _renderer.SetViewport(viewport.X, viewport.Y, viewport.Width, viewport.Height);

        Color4 clearColor = frame.Setup.BackgroundColor;
        _renderer.Clear(clearColor);

        Vector2 cameraSize = frame.Setup.CameraSize;

        float cameraExtendX = cameraSize.X * 0.5f;
        float cameraExtendY = cameraSize.Y * 0.5f;
        Matrix4 projection = Matrix4.CreateOrthographicOffCenter(-cameraExtendX, cameraExtendX, -cameraExtendY, cameraExtendY, 0.0f, 1.0f);

        Matrix4 view = Matrix4.CreateScale(1, 1, 1) * Matrix4.CreateTranslation(0, 0, 0);

        Matrix4 viewProject = view * projection;

        _renderer.DrawQuad(viewProject, 0, 0, cameraSize.X, cameraSize.Y, Color4.DarkSlateGray);
        _renderer.DrawQuad(viewProject, 0, 0, cameraSize.X, cameraSize.Y, _testTexture, Color4.White);

        _renderer.DrawLine(viewProject, -cameraSize.X, 0.0f, cameraSize.X, 0.0f, 2.0f * lineScale, Color4.Red);
        _renderer.DrawLine(viewProject, 0.0f, -cameraSize.Y, 0.0f, cameraSize.Y, 2.0f * lineScale, Color4.Blue);

        if (IsMouseInside)
        {
            Vector4 vp = new Vector4(viewport.X, viewport.Y, viewport.Width, viewport.Height);
            Vector2 mouseWorld = GameMath.Unproject(CurrentMousePos, view, projection, vp, winSize);
            Vector2 cursorSize = new Vector2(16, 16);
            _renderer.DrawQuad(viewProject, mouseWorld + new Vector2(cursorSize.X, -cursorSize.Y) * 0.5f, cursorSize, _mouseArrowTexture);
        }

        _renderer.DrawRectangle(viewProject, 0, 0, cameraSize.X, cameraSize.Y, 4.0f * lineScale, Color4.Yellow);

        _renderer.CheckForErrors();
    }

    private bool _disposed = false;
    public void Dispose()
    {
        if (!_disposed)
        {
            _activeFrame = null;
            _activeFrameId = Guid.Empty;

            _frameManager.ClearFrames();
            _gameObjectManager.ClearObjects();

            _testTexture?.Dispose();
            _mouseArrowTexture?.Dispose();

            _renderer.Dispose();

            _disposed = true;
        }
    }


}