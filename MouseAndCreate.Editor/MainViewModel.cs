using DevExpress.Mvvm;
using MouseAndCreate.Input;
using MouseAndCreate.Play;
using OpenTK.Mathematics;
using System;

namespace MouseAndCreate.Editor;

public class MainViewModel : ViewModelBase
{
    public DelegateCommand OnWindowLoadedCommand { get; }
    public DelegateCommand OnWindowUnloadedCommand { get; }

    public GameEditor Editor { get => GetValue<GameEditor>(); private set => SetValue(value); }

    public string CameraInfo { get => GetValue<string>(); private set => SetValue(value); }

    private IGameInputManager _gameInputMng = null;

    private Vector2i _glControlSize = Vector2i.Zero;
    private Vector2 _glControlMousePos = Vector2.Zero;
    private bool _glControlIsInside = false;

    public MainViewModel()
    {
        OnWindowLoadedCommand = new DelegateCommand(OnWindowLoaded);
        OnWindowUnloadedCommand = new DelegateCommand(OnWindowUnloaded);
    }

    private void OnWindowLoaded()
    {
        Editor = new GameEditor();
        CameraInfo = Editor.Camera.ToString();
        Editor.Camera.Changed += OnEditorCameraChanged;

        Frames.IFrame frame = Editor.Frames.AddFrame();
        Editor.ActiveFrameId = frame.Id;

        _gameInputMng = Editor as IGameInputManager;
        if (_glControlIsInside)
            _gameInputMng.MouseEnter();
        else
            _gameInputMng.MouseLeave();
        _gameInputMng.MouseMove(_glControlMousePos);

        Editor.Resize(_glControlSize);
    }

    private void OnEditorCameraChanged(ICamera camera)
        => CameraInfo = Editor.Camera.ToString();

    private void OnWindowUnloaded()
    {
        Editor?.Dispose();
        Editor = null;
    }

    public void GameMouseMove(Vector2 mousePos)
    {
        _glControlMousePos = mousePos;

        if (_gameInputMng is not null)
            _gameInputMng.MouseMove(mousePos);
    }

    public void GameUpdateAndRender(TimeSpan deltaTime)
    {
        if (Editor is not null)
        {
            Editor.Update(deltaTime);
            Editor.Render(deltaTime);
        }
    }

    public void GameResize(Vector2i size)
    {
        _glControlSize = size;

        if (Editor is not null)
            Editor.Resize(size);
    }

    public void GameMouseEnter()
    {
        _glControlIsInside = true;

        if (_gameInputMng is not null)
            _gameInputMng.MouseEnter();
    }

    public void GameMouseLeave()
    {
        _glControlIsInside = false;

        if (_gameInputMng is not null)
            _gameInputMng.MouseLeave();
    }

    public void GameMouseButtonDown(Vector2 position, MouseButton button)
    {
        if (_gameInputMng is not null)
            _gameInputMng.MouseButtonDown(position, button);
    }

    public void GameMouseButtonUp(Vector2 position, MouseButton button)
    {
        if (_gameInputMng is not null)
            _gameInputMng.MouseButtonUp(position, button);
    }

    public void GameMouseWheel(Vector2 position, Vector2 offset)
    {
        if (_gameInputMng is not null)
            _gameInputMng.MouseWheel(position, offset);
    }

    public void GameKeyDown(Key key, KeyModifiers modifiers, bool isRepeat)
    {
        if (_gameInputMng is not null)
            _gameInputMng.KeyDown(key, modifiers, isRepeat);
    }

    public void GameKeyUp(Key key, KeyModifiers modifiers, bool wasRepeat)
    {
        if (_gameInputMng is not null)
            _gameInputMng.KeyUp(key, modifiers, wasRepeat);
    }

    public void GameKeyInput(string input)
    {
        if (_gameInputMng is not null)
            _gameInputMng.Input(input);
    }
}
