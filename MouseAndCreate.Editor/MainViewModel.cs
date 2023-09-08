using DevExpress.Mvvm;
using MouseAndCreate.Editor.Services;
using MouseAndCreate.Frames;
using MouseAndCreate.Input;
using MouseAndCreate.Platform;
using MouseAndCreate.Play;
using OpenTK.Mathematics;
using System;
using System.Collections.ObjectModel;

namespace MouseAndCreate.Editor;

public class MainViewModel : ViewModelBase, IWindowManager
{
    public GameEditor Editor { get => GetValue<GameEditor>(); private set => SetValue(value); }
    private IGameInputManager _gameInputMng = null;
    private Vector2i _glControlSize = Vector2i.Zero;
    private Vector2 _glControlMousePos = Vector2.Zero;
    private bool _glControlIsInside = false;

    public CursorType Cursor { get => GetValue<CursorType>(); set => SetValue(value); }

    public int SelectedEditorIndex { get => GetValue<int>(); set => SetValue(value, () => SelectedEditorIndexChanged(value)); }

    public FrameViewModel ActiveFrame { get => GetValue<FrameViewModel>(); set => SetValue(value, () => ActiveFrameChanged(value)); }
    public ObservableCollection<FrameViewModel> Frames { get; }

    public DelegateCommand OnWindowLoadedCommand { get; }
    public DelegateCommand OnWindowUnloadedCommand { get; }
    public DelegateCommand OnGLReadyCommand { get; }

    public DelegateCommand ShowStoryBoardEditorCommand { get; }
    public DelegateCommand ShowLevelEditorCommand { get; }
    public DelegateCommand ShowEventEditorCommand { get; }

    public DelegateCommand AddFrameCommand { get; }
    public DelegateCommand<FrameViewModel> RemoveFrameCommand { get; }

    public MainViewModel()
    {
        Frames = new ObservableCollection<FrameViewModel>();

        SelectedEditorIndex = 0;
        OnWindowLoadedCommand = new DelegateCommand(OnWindowLoaded);
        OnWindowUnloadedCommand = new DelegateCommand(OnWindowUnloaded);
        OnGLReadyCommand = new DelegateCommand(OnGLReady);

        ShowStoryBoardEditorCommand = new DelegateCommand(ShowStoryBoardEditor, CanShowStoryBoardEditor);
        ShowLevelEditorCommand = new DelegateCommand(ShowLevelEditor, CanShowLevelEditor);
        ShowEventEditorCommand = new DelegateCommand(ShowEventEditor, CanShowEventEditor);

        AddFrameCommand = new DelegateCommand(AddFrame);
        RemoveFrameCommand = new DelegateCommand<FrameViewModel>(RemoveFrame, CanRemoveFrame);
    }

    private void ActiveFrameChanged(FrameViewModel frame)
    {
        if (frame is not null)
        {
            if (!Editor.ActiveFrameId.Equals(frame.Id))
            {
                Editor.ActiveFrameId = frame.Id;
            }
        }
        else
            Editor.ActiveFrameId = Guid.Empty;

        ShowStoryBoardEditorCommand.RaiseCanExecuteChanged();
        ShowLevelEditorCommand.RaiseCanExecuteChanged();
        ShowEventEditorCommand.RaiseCanExecuteChanged();
    }

    private void OnWindowLoaded()
    {
        
    }

    private void OnGLReady()
    {
        IControlInputQueryService inputQueryService = GetService<IControlInputQueryService>();
        if (inputQueryService is null)
            throw new Exception("Input query service not registered!");

        Frames.Clear();

        Editor = new GameEditor(this, inputQueryService);

        //IFrame frame = Editor.Frames.AddFrame();
        //ActiveFrame = frame;
        //Frames.Add(new FrameViewModel(frame));

        _gameInputMng = Editor as IGameInputManager;
        if (_glControlIsInside)
            _gameInputMng.MouseEnter();
        else
            _gameInputMng.MouseLeave();
        _gameInputMng.MouseMove(_glControlMousePos);

        IGame game = Editor;

        game.Resize(_glControlSize);

        Editor.Initialize();
    }

    private void SelectedEditorIndexChanged(int value)
    {
        ShowStoryBoardEditorCommand.RaiseCanExecuteChanged();
        ShowLevelEditorCommand.RaiseCanExecuteChanged();
        ShowEventEditorCommand.RaiseCanExecuteChanged();
    }

    private bool CanShowStoryBoardEditor() => SelectedEditorIndex != 0;
    private void ShowStoryBoardEditor()
    {
        SelectedEditorIndex = 0;
    }

    private bool CanShowLevelEditor() => SelectedEditorIndex != 1 && ActiveFrame is not null;
    private void ShowLevelEditor()
    {
        SelectedEditorIndex = 1;
    }

    private bool CanShowEventEditor() => SelectedEditorIndex != 2 && ActiveFrame is not null;
    private void ShowEventEditor()
    {
        SelectedEditorIndex = 2;
    }

    private void AddFrame()
    {
        int frameCount = Editor.Frames.Frames.Count;
        string name = $"Frame-{++frameCount}";
        IFrame frame = Editor.Frames.AddFrame(name);
        FrameViewModel frameViewModel = new FrameViewModel(frame);
        Frames.Add(frameViewModel);
        ActiveFrame = frameViewModel;
    }

    private bool CanRemoveFrame(FrameViewModel frame) => frame is not null && Frames.Contains(frame);
    private void RemoveFrame(FrameViewModel frame)
    {
        if (Editor.Frames.RemoveFrame(frame.Id))
            Frames.Remove(frame);
    }

    private void OnWindowUnloaded()
    {
        Editor.Release();
        Editor?.Dispose();
        Editor = null;
    }

    CursorType ICursorManager.GetCursor() => Cursor;

    void ICursorManager.SetCursor(CursorType cursor) => Cursor = cursor;

    public void GameMouseMove(Vector2 mousePos)
    {
        _glControlMousePos = mousePos;

        if (_gameInputMng is not null)
            _gameInputMng.MouseMove(mousePos);
    }

    public void GameUpdateAndRender(TimeSpan deltaTime)
    {
        if (Editor is IGame game)
        {
            game.Update(game.Renderer, deltaTime);
            game.Render(game.Renderer, deltaTime);
        }
    }

    public void GameResize(Vector2i size)
    {
        _glControlSize = size;

        if (Editor is IGame game)
            game.Resize(size);
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
