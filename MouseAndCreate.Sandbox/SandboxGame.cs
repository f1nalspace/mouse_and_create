using MouseAndCreate.Configurations;
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
        ImageFlags imageFlags = _coordinateSystem == CoordinateSystem.Cartesian ? ImageFlags.FlipY : ImageFlags.None;

        _mouseArrowTexture = _renderer.LoadTexture(DefaultTextures.MouseArrow, TextureFormat.RGBA8, imageFlags);
        _testTexture = _renderer.LoadTexture(DefaultTextures.OpenGLTestTexture, TextureFormat.RGBA8, imageFlags);

        //new[] { CodePointRange.BasicLatin }

        using Stream defaultFontStream = DefaultFonts.SulphurPointRegular;
        _defaultFont = LoadFont(_renderer, nameof(DefaultFonts.SulphurPointRegular), defaultFontStream, 32, new[] { CodePointRange.BasicLatin }, imageFlags);

        using Stream consoleFontStream = DefaultFonts.BitstreamVeraSansMono;
        _consoleFont = LoadFont(_renderer, nameof(DefaultFonts.BitstreamVeraSansMono), consoleFontStream, 16, new[] { CodePointRange.BasicLatin }, imageFlags);
    }

    public override void Render(TimeSpan deltaTime)
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

        _renderer.DrawQuad(viewProject, 0, 0, cameraSize.X, cameraSize.Y, Color4.Yellow);
        _renderer.DrawQuad(viewProject, 0, 0, cameraSize.X, cameraSize.Y, _testTexture, Color4.White);

        _renderer.DrawLine(viewProject, -cameraSize.X, 0.0f, cameraSize.X, 0.0f, 2.0f * lineScale, Color4.Red);
        _renderer.DrawLine(viewProject, 0.0f, -cameraSize.Y, 0.0f, cameraSize.Y, 2.0f * lineScale, Color4.Blue);

        string testText = "Bitstream Vera Sans\nHallo Welt!";
        Vector2 testTextSize = _renderer.MeasureString(testText, _consoleFont);
        Vector2 testTextPos = new Vector2(0, 0) + testTextSize * 0.5f;

        _renderer.DrawRectangle(viewProject, testTextPos, testTextSize, 2.0f * lineScale, Color4.GreenYellow);

        _renderer.DrawString(viewProject, new Vector2(0, 0), testText, _consoleFont);

        if (IsMouseInside)
        {
            Vector4 vp = new Vector4(viewport.X, viewport.Y, viewport.Width, viewport.Height);
            Vector2 mouseWorld = GameMath.Unproject(CurrentMousePos, view, projection, vp, winSize);
            Vector2 cursorSize = new Vector2(16, 16);

            _renderer.DrawQuad(viewProject, mouseWorld + new Vector2(cursorSize.X, cursorSize.Y * -upDirection) * 0.5f, cursorSize, _mouseArrowTexture);
        }

        _renderer.DrawRectangle(viewProject, 0, 0, cameraSize.X, cameraSize.Y, 4.0f * lineScale, Color4.Yellow);
    }

    protected override void DisposeResources()
    {
        _consoleFont?.Dispose();
        _defaultFont?.Dispose();

        _testTexture?.Dispose();
        _mouseArrowTexture?.Dispose();
    }
}
