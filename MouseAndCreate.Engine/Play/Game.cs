using MouseAndCreate.Configurations;
using MouseAndCreate.Fonts;
using MouseAndCreate.Frames;
using MouseAndCreate.Graphics;
using MouseAndCreate.Input;
using MouseAndCreate.Objects;
using MouseAndCreate.Platform;
using MouseAndCreate.Rendering;
using MouseAndCreate.Textures;
using MouseAndCreate.Types;
using OpenTK.Mathematics;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace MouseAndCreate.Play;

public class Game : IGame, IGameInputManager, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private readonly IGameSetup _setup;
    private readonly IGameObjectManager _gameObjectManager;
    private readonly IFrameManager _frameManager;
    private readonly ICamera _camera;
    private readonly InputState _inputState;

    protected readonly IRenderer _renderer;

    protected readonly IWindowManager _windowMng;
    protected readonly IInputQuery _inputQuery;

    public IGameSetup Setup => _setup;
    public IGameObjectManager Objects => _gameObjectManager;
    public IFrameManager Frames => _frameManager;
    public ICamera Camera => _camera;
    public InputState InputState => _inputState;

    public Guid ActiveFrameId { get => _activeFrameId; set => ChangeFrameById(value); }
    private Guid _activeFrameId = Guid.Empty;
    public event ActiveFrameChangedEventHandler ActiveFrameChanged;

    private IFrame _activeFrame = null;
    private readonly object _activeFrameLock = new object();

    public Vector2i WindowSize { get; private set; } = Vector2i.Zero;
    public Vector2 CurrentMousePos { get; set; } = new Vector2i(-1000, -1000);
    public bool IsMouseInside { get; set; } = true;

    protected readonly CoordinateSystem _coordinateSystem;

    protected static IFontTexture LoadFont(IRenderer renderer, string name, Stream fontStream, float fontSize, CodePointRange[] ranges, ImageFlags imageFlags, int textureWidth = 512, int textureHeight = 512)
    {
        IFontBuilderFactory fontBuilderFactory = new DefaultFontBuilderFactory();
        IBitmapFontBuilder fontBuilder = fontBuilderFactory.Create();
        IBitmapFontBuilderContext builderCtx = fontBuilder.Begin(textureWidth, textureHeight);
        fontBuilder.Add(builderCtx, name, fontStream, 0, fontSize, ranges);
        using BitmapFont fontBitmap = fontBuilder.End(builderCtx, imageFlags);
        byte[] rgba = ImageConverter.ConvertAlphaToRGBA(fontBitmap.Image.Width, fontBitmap.Image.Height, fontBitmap.Image.Data, false);
        TextureData textureData = new TextureData(fontBitmap.Image.Width, fontBitmap.Image.Height, rgba, TextureFormat.RGBA8);
        IFontTexture result = renderer.LoadFont(name, fontBitmap, textureData);
        return result;
    }

    public Game(IWindowManager windowMng, IInputQuery inputQuery, GameSetup setup = null)
    {
        _windowMng = windowMng ?? throw new ArgumentNullException(nameof(windowMng));
        _inputQuery = inputQuery ?? throw new ArgumentNullException(nameof(inputQuery));

        _setup = (setup ??= GameSetup.Default);

        _coordinateSystem = _setup.CoordinateSystem;

        IRendererFactory rendererFactory = new RendererFactory();
        _renderer = rendererFactory.Create(setup.Renderer, _coordinateSystem);

        _gameObjectManager = new GameObjectManager();

        _frameManager = new FrameManager(this);

        _camera = new Camera(setup.CameraSize);
        _camera.Changed += delegate (ICamera camera)
        {
            RaisePropertyChanged(nameof(Camera));
        };

        _inputState = new InputState();

        _renderer.Init();


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

    protected virtual void KeyDown(Key key, KeyModifiers modifiers, bool isRepeat) { }
    void IKeyboardInputManager.KeyDown(Key key, KeyModifiers modifiers, bool isRepeat)
    {
        if (key != Key.None)
        {
            Debug.WriteLine($"Key down '{key}', mods: '{modifiers}', repeat: {(isRepeat ? "yes" : "no")}");
            KeyDown(key, modifiers, isRepeat);
        }
    }

    protected virtual void KeyUp(Key key, KeyModifiers modifiers, bool wasRepeat) { }
    void IKeyboardInputManager.KeyUp(Key key, KeyModifiers modifiers, bool wasRepeat)
    {
        if (key != Key.None)
        {
            Debug.WriteLine($"Key up '{key}', mods: '{modifiers}', repeat: {(wasRepeat ? "yes" : "no")}");
            KeyUp(key, modifiers, wasRepeat);
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
        _renderer.SetViewport(0, 0, WindowSize.X, WindowSize.Y);
        _renderer.Clear(Color4.Black);
    }

    protected virtual void DisposeResources() { }

    private bool _disposed = false;
    public void Dispose()
    {
        if (!_disposed)
        {
            _activeFrame = null;
            _activeFrameId = Guid.Empty;

            _frameManager.ClearFrames();
            _gameObjectManager.ClearObjects();

            DisposeResources();

            _renderer.Dispose();

            _disposed = true;
        }
    }


}
