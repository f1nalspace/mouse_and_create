﻿using System;
using MouseAndCreate.Configurations;
using MouseAndCreate.Frames;
using MouseAndCreate.Input;
using MouseAndCreate.Objects;
using OpenTK.Mathematics;

namespace MouseAndCreate.Play;

public interface IGame : IDisposable
{
    IGameSetup Setup { get; }

    Guid ActiveFrameId { get; set; }
    IFrameManager Frames { get; }
    IGameObjectManager Objects { get; }
    ICamera Camera { get; }
    InputState InputState { get; }

    Vector2i WindowSize { get; }
    Vector2 CurrentMousePos { get; }
    bool IsMouseInside { get; }

    event ActiveFrameChangedEventHandler ActiveFrameChanged;

    void Initialize();
    void Release();
    void Update(TimeSpan deltaTime);
    void Render(TimeSpan deltaTime);
    void Resize(Vector2i newSize);
}

public delegate void ActiveFrameChangedEventHandler(IGame game, IFrame frame);
