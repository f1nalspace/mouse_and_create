using System;
using MouseAndCreate.Configurations;
using MouseAndCreate.Frames;
using MouseAndCreate.Input;
using MouseAndCreate.Objects;
using MouseAndCreate.Platform;
using MouseAndCreate.Rendering;
using OpenTK.Mathematics;

namespace MouseAndCreate.Play;

public interface IGame : IContentLoader, IDisposable
{
    IGameSetup Setup { get; }

    Guid ActiveFrameId { get; set; }
    IFrameManager Frames { get; }
    IGameObjectManager Objects { get; }
    ICamera Camera { get; }
    InputState InputState { get; }
    IRenderer Renderer { get; }

    Vector2i WindowSize { get; }
    Vector2 CurrentMousePos { get; }
    bool IsMouseInside { get; }

    event ActiveFrameChangedEventHandler ActiveFrameChanged;

    CursorType Cursor { get; set; }

    void Initialize();
    void Release();
    void Update(IRenderer renderer, TimeSpan deltaTime);
    void Render(IRenderer renderer, TimeSpan deltaTime);
    void Resize(Vector2i newSize);
}

public delegate void ActiveFrameChangedEventHandler(IGame game, IFrame frame);
