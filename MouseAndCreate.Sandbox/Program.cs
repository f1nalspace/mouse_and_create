using MouseAndCreate.Configurations;
using MouseAndCreate.Frames;
using MouseAndCreate.Input;
using MouseAndCreate.Play;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Diagnostics;

using OTKButton = OpenTK.Windowing.GraphicsLibraryFramework.MouseButton;
using OTKKeys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;
using IK = MouseAndCreate.Input.Key;
using MouseAndCreate.Platform;
using OpenTK.Windowing.Common.Input;

namespace MouseAndCreate;

class Program
{
    static IGame _game = null;
    static IGameInputManager _inputMng = null;

    static MouseButton Translate(OTKButton button)
    {
        return button switch
        {
            OTKButton.Button1 => MouseButton.Left,
            OTKButton.Button2 => MouseButton.Middle,
            OTKButton.Button3 => MouseButton.Right,
            _ => MouseButton.None
        };
    }

    static IK TranslateGeneric(OTKKeys keys)
    {
        return keys switch
        {
            OTKKeys.Space => IK.Space,
            OTKKeys.D0 => IK.D0,
            OTKKeys.D1 => IK.D1,
            OTKKeys.D2 => IK.D2,
            OTKKeys.D3 => IK.D3,
            OTKKeys.D4 => IK.D4,
            OTKKeys.D5 => IK.D5,
            OTKKeys.D6 => IK.D6,
            OTKKeys.D7 => IK.D7,
            OTKKeys.D8 => IK.D8,
            OTKKeys.D9 => IK.D9,
            OTKKeys.A => IK.A,
            OTKKeys.B => IK.B,
            OTKKeys.C => IK.C,
            OTKKeys.D => IK.D,
            OTKKeys.E => IK.E,
            OTKKeys.F => IK.F,
            OTKKeys.G => IK.G,
            OTKKeys.H => IK.H,
            OTKKeys.I => IK.I,
            OTKKeys.J => IK.J,
            OTKKeys.K => IK.K,
            OTKKeys.L => IK.L,
            OTKKeys.M => IK.M,
            OTKKeys.N => IK.N,
            OTKKeys.O => IK.O,
            OTKKeys.P => IK.P,
            OTKKeys.Q => IK.Q,
            OTKKeys.R => IK.R,
            OTKKeys.S => IK.S,
            OTKKeys.T => IK.T,
            OTKKeys.U => IK.U,
            OTKKeys.V => IK.V,
            OTKKeys.W => IK.W,
            OTKKeys.X => IK.X,
            OTKKeys.Y => IK.Y,
            OTKKeys.Z => IK.Z,
            OTKKeys.Escape => IK.Escape,
            OTKKeys.Enter => IK.Enter,
            OTKKeys.Tab => IK.Tab,
            OTKKeys.Backspace => IK.Backspace,
            OTKKeys.Insert => IK.Insert,
            OTKKeys.Delete => IK.Delete,
            OTKKeys.Right => IK.Right,
            OTKKeys.Left => IK.Left,
            OTKKeys.Down => IK.Down,
            OTKKeys.Up => IK.Up,
            OTKKeys.PageUp => IK.PageUp,
            OTKKeys.PageDown => IK.PageDown,
            OTKKeys.Home => IK.Home,
            OTKKeys.End => IK.End,
            OTKKeys.CapsLock => IK.CapsLock,
            OTKKeys.ScrollLock => IK.ScrollLock,
            OTKKeys.NumLock => IK.NumLock,
            OTKKeys.PrintScreen => IK.PrintScreen,
            OTKKeys.Pause => IK.Pause,
            OTKKeys.F1 => IK.F1,
            OTKKeys.F2 => IK.F2,
            OTKKeys.F3 => IK.F3,
            OTKKeys.F4 => IK.F4,
            OTKKeys.F5 => IK.F5,
            OTKKeys.F6 => IK.F6,
            OTKKeys.F7 => IK.F7,
            OTKKeys.F8 => IK.F8,
            OTKKeys.F9 => IK.F9,
            OTKKeys.F10 => IK.F10,
            OTKKeys.F11 => IK.F11,
            OTKKeys.F12 => IK.F12,
            OTKKeys.F13 => IK.F13,
            OTKKeys.F14 => IK.F14,
            OTKKeys.F15 => IK.F15,
            OTKKeys.F16 => IK.F16,
            OTKKeys.F17 => IK.F17,
            OTKKeys.F18 => IK.F18,
            OTKKeys.F19 => IK.F19,
            OTKKeys.F20 => IK.F20,
            OTKKeys.F21 => IK.F21,
            OTKKeys.F22 => IK.F22,
            OTKKeys.F23 => IK.F23,
            OTKKeys.F24 => IK.F24,
            OTKKeys.KeyPad0 => IK.NumPad0,
            OTKKeys.KeyPad1 => IK.NumPad1,
            OTKKeys.KeyPad2 => IK.NumPad2,
            OTKKeys.KeyPad3 => IK.NumPad3,
            OTKKeys.KeyPad4 => IK.NumPad4,
            OTKKeys.KeyPad5 => IK.NumPad5,
            OTKKeys.KeyPad6 => IK.NumPad6,
            OTKKeys.KeyPad7 => IK.NumPad7,
            OTKKeys.KeyPad8 => IK.NumPad8,
            OTKKeys.KeyPad9 => IK.NumPad9,
            OTKKeys.KeyPadDecimal => IK.NumPadDecimal,
            OTKKeys.KeyPadDivide => IK.NumPadDivide,
            OTKKeys.KeyPadMultiply => IK.NumPadMultiply,
            OTKKeys.KeyPadSubtract => IK.NumPadSubstract,
            OTKKeys.KeyPadAdd => IK.NumPadAdd,
            OTKKeys.KeyPadEnter => IK.NumPadEnter,
            OTKKeys.KeyPadEqual => IK.NumPadEqual,
            OTKKeys.LeftShift => IK.LeftShift,
            OTKKeys.LeftControl => IK.LeftControl,
            OTKKeys.LeftAlt => IK.LeftAlt,
            OTKKeys.LeftSuper => IK.LeftSuper,
            OTKKeys.RightShift => IK.RightShift,
            OTKKeys.RightControl => IK.RightControl,
            OTKKeys.RightAlt => IK.RightAlt,
            OTKKeys.RightSuper => IK.RightSuper,
            OTKKeys.Menu => IK.WinMenu,
            _ => IK.None
        };
    }

    static IK TranslateUS(OTKKeys keys)
    {
        IK result = TranslateGeneric(keys);
        if (result == IK.None)
        {
            // TODO(final): OpenTK US-Keyboard mapping!
        }
        return result;
    }

    static KeyModifiers GetModifiers(KeyboardKeyEventArgs args)
    {
        KeyModifiers result = KeyModifiers.None;
        if (args.Alt) result |= KeyModifiers.Alt;
        if (args.Control) result |= KeyModifiers.Ctrl;
        if (args.Shift) result |= KeyModifiers.Shift;
        return result;
    }

    class GameWindowInputQuery : IInputQuery
    {
        private readonly GameWindow _window;

        public GameWindowInputQuery(GameWindow window) 
        {
            _window = window;
        }

        public Vector2 GetMousePosition() => _window.MousePosition;
    }

    class GameWindowManager : IWindowManager
    {
        private readonly GameWindow _window;

        public GameWindowManager(GameWindow window)
        {
            _window = window;
        }

        public CursorType GetCursor()
        {
            if (_window.Cursor == MouseCursor.Default)
                return CursorType.Arrow;
            else if (_window.Cursor == MouseCursor.Crosshair)
                return CursorType.Crosshair;
            else if (_window.Cursor == MouseCursor.HResize)
                return CursorType.HResize;
            else if (_window.Cursor == MouseCursor.VResize)
                return CursorType.VResize;
            else if (_window.Cursor == MouseCursor.IBeam)
                return CursorType.IBeam;
            else
                return CursorType.Arrow;
        }

        public void SetCursor(CursorType cursor)
        {
            _window.Cursor = cursor switch
            {
                CursorType.Hand => MouseCursor.Hand,
                CursorType.Crosshair => MouseCursor.Crosshair,
                CursorType.HResize => MouseCursor.HResize,
                CursorType.VResize => MouseCursor.VResize,
                CursorType.IBeam => MouseCursor.IBeam,
                _ => MouseCursor.Default,
            };
        }
    }

    static void Main(string[] args)
    {
        GameSetup setup = new GameSetup(new Vector2i(1280, 720));

        using GameWindow window = new GameWindow(GameWindowSettings.Default, new NativeWindowSettings()
        {
            Title = setup.Title,
            Size = setup.WindowSize,
        });

        GameWindowInputQuery inputQuery = new GameWindowInputQuery(window);

        GameWindowManager windowMng = new GameWindowManager(window);

        window.Resize += delegate (ResizeEventArgs args) { _game.Resize(args.Size); };

        window.RenderFrame += delegate (FrameEventArgs args)
        {
            _game.Render(TimeSpan.FromSeconds(args.Time));
            window.SwapBuffers();
        };

        window.UpdateFrame += delegate (FrameEventArgs args)
        {
            _game.Update(TimeSpan.FromSeconds(args.Time));
        };

        window.Load += delegate ()
        {
            _game = new Game(windowMng, inputQuery);
            IFrame frame = _game.Frames.AddFrame();
            _game.ActiveFrameId = frame.Id;
            _inputMng = _game as IGameInputManager;
        };

        window.Unload += delegate ()
        {
            _game.Dispose();
            _game = null;
        };

        window.MouseMove += delegate (MouseMoveEventArgs args) { _inputMng.MouseMove(args.Position); };
        window.MouseEnter += delegate () { _inputMng.MouseEnter(); };
        window.MouseLeave += delegate () { _inputMng.MouseLeave(); };
        window.MouseDown += delegate (MouseButtonEventArgs args) { _inputMng.MouseButtonDown(window.MousePosition, Translate(args.Button)); };
        window.MouseUp += delegate (MouseButtonEventArgs args) { _inputMng.MouseButtonUp(window.MousePosition, Translate(args.Button)); };
        window.MouseWheel += delegate (MouseWheelEventArgs args) { _inputMng.MouseWheel(window.MousePosition, args.Offset); };

        window.KeyDown += delegate (KeyboardKeyEventArgs args)
        {
            IK k = TranslateUS(args.Key);
            if (k == IK.None)
                Debug.WriteLine($"Unknown key '{args.Key}' for press");
            KeyModifiers modifiers = GetModifiers(args);
            _inputMng.KeyDown(k, modifiers, args.IsRepeat);
        };
        window.KeyUp += delegate (KeyboardKeyEventArgs args)
        {
            IK k = TranslateUS(args.Key);
            if (k == IK.None)
                Debug.WriteLine($"Unknown key '{args.Key}' for release");
            KeyModifiers modifiers = GetModifiers(args);
            _inputMng.KeyUp(k, modifiers, args.IsRepeat);
        };

        window.Run();
    }
}