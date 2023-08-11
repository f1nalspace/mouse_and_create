using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using IK = MouseAndCreate.Input.Key;
using WK = System.Windows.Input.Key;

namespace MouseAndCreate.Editor
{
    /// <summary>
    /// Interaction logic for LevelEditorWindow.xaml
    /// </summary>
    public partial class LevelEditorWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public LevelEditorWindow()
        {
            InitializeComponent();

            _viewModel = DataContext as MainViewModel;

            glControl.Start(new OpenTK.Wpf.GLWpfControlSettings() { MajorVersion = 3, MinorVersion = 3 });

            glControl.Render += OnGlControlRender;

            glControl.SizeChanged += OnGlControlSizeChanged;

            glControl.MouseMove += OnGlControlMouseMove;
            glControl.MouseLeave += OnGlControlMouseLeave;
            glControl.MouseEnter += OnGlControlMouseEnter;
            glControl.MouseDown += OnGlControlMouseDown;
            glControl.MouseUp += OnGlControlMouseUp;
            glControl.MouseWheel += OnGlControlMouseWheel;
            glControl.KeyDown += OnGlControlKeyDown;
            glControl.KeyUp += OnGlControlKeyUp;
            glControl.TextInput += OnGlControlTextInput;
        }

        private static Input.MouseButton Translate(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => Input.MouseButton.Left,
                MouseButton.Middle => Input.MouseButton.Middle,
                MouseButton.Right => Input.MouseButton.Right,
                _ => Input.MouseButton.None
            };
        }

        private static IK TranslateGeneric(WK key)
        {
            return key switch
            {
                WK.Space => IK.Space,
                WK.D0 => IK.D0,
                WK.D1 => IK.D1,
                WK.D2 => IK.D2,
                WK.D3 => IK.D3,
                WK.D4 => IK.D4,
                WK.D5 => IK.D5,
                WK.D6 => IK.D6,
                WK.D7 => IK.D7,
                WK.D8 => IK.D8,
                WK.D9 => IK.D9,
                WK.A => IK.A,
                WK.B => IK.B,
                WK.C => IK.C,
                WK.D => IK.D,
                WK.E => IK.E,
                WK.F => IK.F,
                WK.G => IK.G,
                WK.H => IK.H,
                WK.I => IK.I,
                WK.J => IK.J,
                WK.K => IK.K,
                WK.L => IK.L,
                WK.M => IK.M,
                WK.N => IK.N,
                WK.O => IK.O,
                WK.P => IK.P,
                WK.Q => IK.Q,
                WK.R => IK.R,
                WK.S => IK.S,
                WK.T => IK.T,
                WK.U => IK.U,
                WK.V => IK.V,
                WK.W => IK.W,
                WK.X => IK.X,
                WK.Y => IK.Y,
                WK.Z => IK.Z,
                WK.Escape => IK.Escape,
                WK.Enter => IK.Enter,
                WK.Tab => IK.Tab,
                WK.Back => IK.Backspace,
                WK.Insert => IK.Insert,
                WK.Delete => IK.Delete,
                WK.Right => IK.Right,
                WK.Left => IK.Left,
                WK.Down => IK.Down,
                WK.Up => IK.Up,
                WK.PageUp => IK.PageUp,
                WK.PageDown => IK.PageDown,
                WK.Home => IK.Home,
                WK.End => IK.End,
                WK.CapsLock => IK.CapsLock,
                WK.Scroll => IK.ScrollLock,
                WK.NumLock => IK.NumLock,
                WK.PrintScreen => IK.PrintScreen,
                WK.Pause => IK.Pause,
                WK.F1 => IK.F1,
                WK.F2 => IK.F2,
                WK.F3 => IK.F3,
                WK.F4 => IK.F4,
                WK.F5 => IK.F5,
                WK.F6 => IK.F6,
                WK.F7 => IK.F7,
                WK.F8 => IK.F8,
                WK.F9 => IK.F9,
                WK.F10 => IK.F10,
                WK.F11 => IK.F11,
                WK.F12 => IK.F12,
                WK.F13 => IK.F13,
                WK.F14 => IK.F14,
                WK.F15 => IK.F15,
                WK.F16 => IK.F16,
                WK.F17 => IK.F17,
                WK.F18 => IK.F18,
                WK.F19 => IK.F19,
                WK.F20 => IK.F20,
                WK.F21 => IK.F21,
                WK.F22 => IK.F22,
                WK.F23 => IK.F23,
                WK.F24 => IK.F24,
                WK.NumPad0 => IK.NumPad0,
                WK.NumPad1 => IK.NumPad1,
                WK.NumPad2 => IK.NumPad2,
                WK.NumPad3 => IK.NumPad3,
                WK.NumPad4 => IK.NumPad4,
                WK.NumPad5 => IK.NumPad5,
                WK.NumPad6 => IK.NumPad6,
                WK.NumPad7 => IK.NumPad7,
                WK.NumPad8 => IK.NumPad8,
                WK.NumPad9 => IK.NumPad9,
                WK.Decimal => IK.NumPadDecimal,
                WK.Divide => IK.NumPadDivide,
                WK.Multiply => IK.NumPadMultiply,
                WK.Subtract => IK.NumPadSubstract,
                WK.Add => IK.NumPadAdd,
                // TODO(final): Keypad-Return to IK.NumPadReturn
                // TODO(final): Keypad-Equal to IK.NumPadEqual
                WK.LeftShift => IK.LeftShift,
                WK.LeftCtrl => IK.LeftControl,
                WK.LeftAlt => IK.LeftAlt,
                WK.LWin => IK.LeftSuper,
                WK.RightShift => IK.RightShift,
                WK.RightCtrl => IK.RightControl,
                WK.RightAlt => IK.RightAlt,
                WK.RWin => IK.RightSuper,
                // TODO(final): Win 'menu' to WinMenu
                _ => IK.None,
            };
        }

        private static IK TranslateUS(WK key)
        {
            IK result = TranslateGeneric(key);
            if (result == IK.None)
            {
                // TODO(final): WPF US-Keyboard mapping!
            }
            return result;
        }

        private void OnGlControlMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(glControl);
            _viewModel.GameMouseButtonDown(new Vector2((float)pos.X, (float)pos.Y), Translate(e.ChangedButton));
        }

        private void OnGlControlMouseUp(object sender, MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(glControl);
            _viewModel.GameMouseButtonUp(new Vector2((float)pos.X, (float)pos.Y), Translate(e.ChangedButton));
        }

        private void OnGlControlMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point pos = e.GetPosition(glControl);
            _viewModel.GameMouseWheel(new Vector2((float)pos.X, (float)pos.Y), new Vector2(0, e.Delta));
        }

        private static string KeyToString(WK key)
        {
            try
            {
                return key.ToString();
            } 
            catch
            {
                return ((int)key).ToString();
            }
        }

        private void OnGlControlKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            IK k = TranslateUS(e.Key);
            if (k == IK.None)
                Debug.WriteLine($"Unknown key '{KeyToString(e.Key)}' for press");
            _viewModel.GameKeyDown(k);
        }

        private void OnGlControlKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            IK k = TranslateUS(e.Key);
            if (k == IK.None)
                Debug.WriteLine($"Unknown key '{KeyToString(e.Key)}' for release");
            _viewModel.GameKeyUp(k);
        }

        private void OnGlControlTextInput(object sender, TextCompositionEventArgs e)
        {
            _viewModel.GameKeyInput(e.Text);
        }

        private void OnGlControlMouseEnter(object sender, MouseEventArgs e)
        {
            _viewModel.GameMouseEnter();
        }

        private void OnGlControlMouseLeave(object sender, MouseEventArgs e)
        {
            _viewModel.GameMouseLeave();
        }

        private void OnGlControlMouseMove(object sender, MouseEventArgs e)
        {
            Point pos = e.GetPosition(glControl);
            _viewModel.GameMouseMove(new Vector2((float)pos.X, (float)pos.Y));
        }

        private void OnGlControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _viewModel.GameResize(new Vector2i((int)e.NewSize.Width, (int)e.NewSize.Height));
        }

        private void OnGlControlRender(TimeSpan deltaTime)
        {
            _viewModel.GameRender(deltaTime);
        }
    }
}
