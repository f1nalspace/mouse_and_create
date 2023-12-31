﻿using MouseAndCreate.Configurations;
using MouseAndCreate.Fonts;
using MouseAndCreate.Frames;
using MouseAndCreate.Graphics;
using MouseAndCreate.Input;
using MouseAndCreate.Platform;
using MouseAndCreate.Play;
using MouseAndCreate.Rendering;
using MouseAndCreate.Textures;
using MouseAndCreate.Types;
using OpenTK.Mathematics;
using System;
using System.IO;

namespace MouseAndCreate;

internal class SandboxGame : Game
{
    protected ITexture _mouseArrowTexture = null;
    protected ITexture _testTexture = null;

    protected IFontTexture _defaultFont = null;
    protected IFontTexture _consoleFont = null;

    public SandboxGame(IWindowManager windowMng, IInputQuery inputQuery, GameSetup setup = null) : base(windowMng, inputQuery, setup)
    {
    }

    protected override void LoadContent()
    {
        ImageFlags imageFlags = _coordinateSystem == CoordinateSystem.Cartesian ? ImageFlags.FlipY : ImageFlags.None;

        _mouseArrowTexture = LoadTexture(DefaultTextures.MouseArrow, TextureFormat.RGBA8, imageFlags);
        _testTexture = LoadTexture(DefaultTextures.OpenGLTestTexture, TextureFormat.RGBA8, imageFlags);

        //new[] { CodePointRange.BasicLatin }

        using Stream defaultFontStream = DefaultFonts.SulphurPointRegular;
        _defaultFont = LoadFont(nameof(DefaultFonts.SulphurPointRegular), defaultFontStream, 32, new[] { CodePointRange.BasicLatin }, imageFlags);

        using Stream consoleFontStream = DefaultFonts.BitstreamVeraSansMono;
        _consoleFont = LoadFont(nameof(DefaultFonts.BitstreamVeraSansMono), consoleFontStream, 16, new[] { CodePointRange.BasicLatin }, imageFlags);
    }

    protected override void Render(IRenderer renderer, TimeSpan deltaTime)
    {
        IFrame frame = null;
        if (!Guid.Empty.Equals(ActiveFrameId))
        {
            frame = Frames.GetFrameById(ActiveFrameId);
        }

        if (frame is null)
        {
            renderer.SetViewport(0, 0, WindowSize.X, WindowSize.Y);
            renderer.Clear(Color4.Black);
            return;
        }

        Vector2i initialSize = Setup.WindowSize;

        Vector2i winSize = WindowSize;

        Ratio frameAspect = frame.Setup.Aspect;

        Viewport viewport = GameMath.ComputeViewport(winSize, initialSize, frameAspect);

        float lineScale = viewport.Scale;

        renderer.SetViewport(viewport.X, viewport.Y, viewport.Width, viewport.Height);

        Color4 clearColor = frame.Setup.BackgroundColor;
        renderer.Clear(clearColor);

        Vector2 cameraSize = frame.Setup.CameraSize;

        float cameraExtendX = cameraSize.X * 0.5f;
        float cameraExtendY = cameraSize.Y * 0.5f;

        Matrix4 projection;
        float upDirection;
        if (_coordinateSystem == CoordinateSystem.Cartesian)
        {
            projection = Matrix4.CreateOrthographicOffCenter(-cameraExtendX, cameraExtendX, -cameraExtendY, cameraExtendY, 0.0f, 1.0f);
            upDirection = 1;
        }
        else
        {
            projection = Matrix4.CreateOrthographicOffCenter(-cameraExtendX, cameraExtendX, cameraExtendY, -cameraExtendY, 0.0f, 1.0f);
            upDirection = -1;
        }

        Matrix4 view = Matrix4.CreateScale(1, 1, 1) * Matrix4.CreateTranslation(0, 0, 0);

        Matrix4 viewProject = view * projection;

        renderer.DrawQuad(viewProject, 0, 0, cameraSize.X, cameraSize.Y, Color4.Yellow);
        renderer.DrawQuad(viewProject, 0, 0, cameraSize.X, cameraSize.Y, _testTexture, Color4.White);

        renderer.DrawLine(viewProject, -cameraSize.X, 0.0f, cameraSize.X, 0.0f, 2.0f * lineScale, Color4.Red);
        renderer.DrawLine(viewProject, 0.0f, -cameraSize.Y, 0.0f, cameraSize.Y, 2.0f * lineScale, Color4.Blue);

        string testText = "Bitstream Vera Sans\nSecond line!";
        Vector2 baseTextPos = new Vector2(5, 5);
        Vector2 testTextSize = renderer.MeasureString(testText, _consoleFont);
        Vector2 testTextPos = baseTextPos + testTextSize * 0.5f;

        renderer.DrawRectangle(viewProject, testTextPos, testTextSize, 2.0f * lineScale, Color4.GreenYellow);

        renderer.DrawString(viewProject, baseTextPos, testText, _consoleFont);

        if (IsMouseInside)
        {
            Vector4 vp = new Vector4(viewport.X, viewport.Y, viewport.Width, viewport.Height);
            Vector2 mouseWorld = GameMath.Unproject(CurrentMousePos, view, projection, vp, winSize);
            Vector2 cursorSize = new Vector2(16, 16);

            renderer.DrawQuad(viewProject, mouseWorld + new Vector2(cursorSize.X, cursorSize.Y * -upDirection) * 0.5f, cursorSize, _mouseArrowTexture);
        }

        renderer.DrawRectangle(viewProject, 0, 0, cameraSize.X, cameraSize.Y, 4.0f * lineScale, Color4.Yellow);
    }
}
