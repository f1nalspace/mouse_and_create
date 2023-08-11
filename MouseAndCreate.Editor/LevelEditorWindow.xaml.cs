using MouseAndCreate.Input;
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
using WinKey = System.Windows.Input.Key;
using WinMouseButton = System.Windows.Input.MouseButton;

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

        private static Input.MouseButton Translate(WinMouseButton button)
        {
            return button switch
            {
                WinMouseButton.Left => Input.MouseButton.Left,
                WinMouseButton.Middle => Input.MouseButton.Middle,
                WinMouseButton.Right => Input.MouseButton.Right,
                _ => Input.MouseButton.None
            };
        }

        private static IK TranslateGeneric(WinKey key)
        {
            return key switch
            {
                WinKey.Space => IK.Space,
                WinKey.D0 => IK.D0,
                WinKey.D1 => IK.D1,
                WinKey.D2 => IK.D2,
                WinKey.D3 => IK.D3,
                WinKey.D4 => IK.D4,
                WinKey.D5 => IK.D5,
                WinKey.D6 => IK.D6,
                WinKey.D7 => IK.D7,
                WinKey.D8 => IK.D8,
                WinKey.D9 => IK.D9,
                WinKey.A => IK.A,
                WinKey.B => IK.B,
                WinKey.C => IK.C,
                WinKey.D => IK.D,
                WinKey.E => IK.E,
                WinKey.F => IK.F,
                WinKey.G => IK.G,
                WinKey.H => IK.H,
                WinKey.I => IK.I,
                WinKey.J => IK.J,
                WinKey.K => IK.K,
                WinKey.L => IK.L,
                WinKey.M => IK.M,
                WinKey.N => IK.N,
                WinKey.O => IK.O,
                WinKey.P => IK.P,
                WinKey.Q => IK.Q,
                WinKey.R => IK.R,
                WinKey.S => IK.S,
                WinKey.T => IK.T,
                WinKey.U => IK.U,
                WinKey.V => IK.V,
                WinKey.W => IK.W,
                WinKey.X => IK.X,
                WinKey.Y => IK.Y,
                WinKey.Z => IK.Z,
                WinKey.Escape => IK.Escape,
                WinKey.Enter => IK.Enter,
                WinKey.Tab => IK.Tab,
                WinKey.Back => IK.Backspace,
                WinKey.Insert => IK.Insert,
                WinKey.Delete => IK.Delete,
                WinKey.Right => IK.Right,
                WinKey.Left => IK.Left,
                WinKey.Down => IK.Down,
                WinKey.Up => IK.Up,
                WinKey.PageUp => IK.PageUp,
                WinKey.PageDown => IK.PageDown,
                WinKey.Home => IK.Home,
                WinKey.End => IK.End,
                WinKey.CapsLock => IK.CapsLock,
                WinKey.Scroll => IK.ScrollLock,
                WinKey.NumLock => IK.NumLock,
                WinKey.PrintScreen => IK.PrintScreen,
                WinKey.Pause => IK.Pause,
                WinKey.F1 => IK.F1,
                WinKey.F2 => IK.F2,
                WinKey.F3 => IK.F3,
                WinKey.F4 => IK.F4,
                WinKey.F5 => IK.F5,
                WinKey.F6 => IK.F6,
                WinKey.F7 => IK.F7,
                WinKey.F8 => IK.F8,
                WinKey.F9 => IK.F9,
                WinKey.F10 => IK.F10,
                WinKey.F11 => IK.F11,
                WinKey.F12 => IK.F12,
                WinKey.F13 => IK.F13,
                WinKey.F14 => IK.F14,
                WinKey.F15 => IK.F15,
                WinKey.F16 => IK.F16,
                WinKey.F17 => IK.F17,
                WinKey.F18 => IK.F18,
                WinKey.F19 => IK.F19,
                WinKey.F20 => IK.F20,
                WinKey.F21 => IK.F21,
                WinKey.F22 => IK.F22,
                WinKey.F23 => IK.F23,
                WinKey.F24 => IK.F24,
                WinKey.NumPad0 => IK.NumPad0,
                WinKey.NumPad1 => IK.NumPad1,
                WinKey.NumPad2 => IK.NumPad2,
                WinKey.NumPad3 => IK.NumPad3,
                WinKey.NumPad4 => IK.NumPad4,
                WinKey.NumPad5 => IK.NumPad5,
                WinKey.NumPad6 => IK.NumPad6,
                WinKey.NumPad7 => IK.NumPad7,
                WinKey.NumPad8 => IK.NumPad8,
                WinKey.NumPad9 => IK.NumPad9,
                WinKey.Decimal => IK.NumPadDecimal,
                WinKey.Divide => IK.NumPadDivide,
                WinKey.Multiply => IK.NumPadMultiply,
                WinKey.Subtract => IK.NumPadSubstract,
                WinKey.Add => IK.NumPadAdd,
                // TODO(final): Keypad-Return to IK.NumPadReturn
                // TODO(final): Keypad-Equal to IK.NumPadEqual
                WinKey.LeftShift => IK.LeftShift,
                WinKey.LeftCtrl => IK.LeftControl,
                WinKey.LeftAlt => IK.LeftAlt,
                WinKey.LWin => IK.LeftSuper,
                WinKey.RightShift => IK.RightShift,
                WinKey.RightCtrl => IK.RightControl,
                WinKey.RightAlt => IK.RightAlt,
                WinKey.RWin => IK.RightSuper,
                // TODO(final): Win 'menu' to WinMenu
                _ => IK.None,
            };
        }

        private static IK TranslateUS(WinKey key)
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

        private static string KeyToString(WinKey key)
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

        private static KeyModifiers TranslateModifiers(ModifierKeys keys)
        {
            KeyModifiers result = KeyModifiers.None;
            if (keys.HasFlag(ModifierKeys.Alt)) result |= KeyModifiers.Alt;
            if (keys.HasFlag(ModifierKeys.Control)) result |= KeyModifiers.Ctrl;
            if (keys.HasFlag(ModifierKeys.Shift)) result |= KeyModifiers.Shift;
            return result;
        }

        private void OnGlControlKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            IK k = TranslateUS(e.Key);
            if (k == IK.None)
                Debug.WriteLine($"Unknown key '{KeyToString(e.Key)}' for press");
            KeyModifiers modifiers = TranslateModifiers(e.KeyboardDevice.Modifiers);
            _viewModel.GameKeyDown(k, modifiers, e.IsRepeat);
        }

        private void OnGlControlKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            IK k = TranslateUS(e.Key);
            if (k == IK.None)
                Debug.WriteLine($"Unknown key '{KeyToString(e.Key)}' for release");
            KeyModifiers modifiers = TranslateModifiers(e.KeyboardDevice.Modifiers);
            _viewModel.GameKeyUp(k, modifiers, e.IsRepeat);
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
            _viewModel.GameUpdateAndRender(deltaTime);
        }
    }
}
