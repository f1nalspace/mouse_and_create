using DevExpress.Mvvm;
using MouseAndCreate.Editor.Services;
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

using IKey = MouseAndCreate.Input.Key;
using IMB = MouseAndCreate.Input.MouseButton;
using WinKey = System.Windows.Input.Key;
using WinMouseButton = System.Windows.Input.MouseButton;

namespace MouseAndCreate.Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = DataContext as MainViewModel;

            ISupportServices supportServices = _viewModel as ISupportServices;

            supportServices.ServiceContainer.RegisterService(new WPFEditorDialogService(this));

            glControl.Ready += OnGlControlReady;

            glControl.Render += OnGlControlRender;

            glControl.Start(new OpenTK.Wpf.GLWpfControlSettings() { MajorVersion = 3, MinorVersion = 3, GraphicsProfile = OpenTK.Windowing.Common.ContextProfile.Compatability });

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

        private void OnGlControlReady()
        {
            _viewModel.OnGLReadyCommand.Execute(null);
        }

        private static IMB Translate(WinMouseButton button)
        {
            return button switch
            {
                WinMouseButton.Left => IMB.Left,
                WinMouseButton.Middle => IMB.Middle,
                WinMouseButton.Right => IMB.Right,
                _ => IMB.None
            };
        }

        private static IKey TranslateGeneric(WinKey key)
        {
            return key switch
            {
                WinKey.Space => IKey.Space,
                WinKey.D0 => IKey.D0,
                WinKey.D1 => IKey.D1,
                WinKey.D2 => IKey.D2,
                WinKey.D3 => IKey.D3,
                WinKey.D4 => IKey.D4,
                WinKey.D5 => IKey.D5,
                WinKey.D6 => IKey.D6,
                WinKey.D7 => IKey.D7,
                WinKey.D8 => IKey.D8,
                WinKey.D9 => IKey.D9,
                WinKey.A => IKey.A,
                WinKey.B => IKey.B,
                WinKey.C => IKey.C,
                WinKey.D => IKey.D,
                WinKey.E => IKey.E,
                WinKey.F => IKey.F,
                WinKey.G => IKey.G,
                WinKey.H => IKey.H,
                WinKey.I => IKey.I,
                WinKey.J => IKey.J,
                WinKey.K => IKey.K,
                WinKey.L => IKey.L,
                WinKey.M => IKey.M,
                WinKey.N => IKey.N,
                WinKey.O => IKey.O,
                WinKey.P => IKey.P,
                WinKey.Q => IKey.Q,
                WinKey.R => IKey.R,
                WinKey.S => IKey.S,
                WinKey.T => IKey.T,
                WinKey.U => IKey.U,
                WinKey.V => IKey.V,
                WinKey.W => IKey.W,
                WinKey.X => IKey.X,
                WinKey.Y => IKey.Y,
                WinKey.Z => IKey.Z,
                WinKey.Escape => IKey.Escape,
                WinKey.Enter => IKey.Enter,
                WinKey.Tab => IKey.Tab,
                WinKey.Back => IKey.Backspace,
                WinKey.Insert => IKey.Insert,
                WinKey.Delete => IKey.Delete,
                WinKey.Right => IKey.Right,
                WinKey.Left => IKey.Left,
                WinKey.Down => IKey.Down,
                WinKey.Up => IKey.Up,
                WinKey.PageUp => IKey.PageUp,
                WinKey.PageDown => IKey.PageDown,
                WinKey.Home => IKey.Home,
                WinKey.End => IKey.End,
                WinKey.CapsLock => IKey.CapsLock,
                WinKey.Scroll => IKey.ScrollLock,
                WinKey.NumLock => IKey.NumLock,
                WinKey.PrintScreen => IKey.PrintScreen,
                WinKey.Pause => IKey.Pause,
                WinKey.F1 => IKey.F1,
                WinKey.F2 => IKey.F2,
                WinKey.F3 => IKey.F3,
                WinKey.F4 => IKey.F4,
                WinKey.F5 => IKey.F5,
                WinKey.F6 => IKey.F6,
                WinKey.F7 => IKey.F7,
                WinKey.F8 => IKey.F8,
                WinKey.F9 => IKey.F9,
                WinKey.F10 => IKey.F10,
                WinKey.F11 => IKey.F11,
                WinKey.F12 => IKey.F12,
                WinKey.F13 => IKey.F13,
                WinKey.F14 => IKey.F14,
                WinKey.F15 => IKey.F15,
                WinKey.F16 => IKey.F16,
                WinKey.F17 => IKey.F17,
                WinKey.F18 => IKey.F18,
                WinKey.F19 => IKey.F19,
                WinKey.F20 => IKey.F20,
                WinKey.F21 => IKey.F21,
                WinKey.F22 => IKey.F22,
                WinKey.F23 => IKey.F23,
                WinKey.F24 => IKey.F24,
                WinKey.NumPad0 => IKey.NumPad0,
                WinKey.NumPad1 => IKey.NumPad1,
                WinKey.NumPad2 => IKey.NumPad2,
                WinKey.NumPad3 => IKey.NumPad3,
                WinKey.NumPad4 => IKey.NumPad4,
                WinKey.NumPad5 => IKey.NumPad5,
                WinKey.NumPad6 => IKey.NumPad6,
                WinKey.NumPad7 => IKey.NumPad7,
                WinKey.NumPad8 => IKey.NumPad8,
                WinKey.NumPad9 => IKey.NumPad9,
                WinKey.Decimal => IKey.NumPadDecimal,
                WinKey.Divide => IKey.NumPadDivide,
                WinKey.Multiply => IKey.NumPadMultiply,
                WinKey.Subtract => IKey.NumPadSubstract,
                WinKey.Add => IKey.NumPadAdd,
                // TODO(final): Keypad-Return to IKey.NumPadReturn
                // TODO(final): Keypad-Equal to IKey.NumPadEqual
                WinKey.LeftShift => IKey.LeftShift,
                WinKey.LeftCtrl => IKey.LeftControl,
                WinKey.LeftAlt => IKey.LeftAlt,
                WinKey.LWin => IKey.LeftSuper,
                WinKey.RightShift => IKey.RightShift,
                WinKey.RightCtrl => IKey.RightControl,
                WinKey.RightAlt => IKey.RightAlt,
                WinKey.RWin => IKey.RightSuper,
                // TODO(final): Win 'menu' to WinMenu
                _ => IKey.None,
            };
        }

        private static IKey TranslateUS(WinKey key)
        {
            IKey result = TranslateGeneric(key);
            if (result == IKey.None)
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
            IKey k = TranslateUS(e.Key);
            if (k == IKey.None)
                Debug.WriteLine($"Unknown key '{KeyToString(e.Key)}' for press");
            KeyModifiers modifiers = TranslateModifiers(e.KeyboardDevice.Modifiers);
            _viewModel.GameKeyDown(k, modifiers, e.IsRepeat);
        }

        private void OnGlControlKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            IKey k = TranslateUS(e.Key);
            if (k == IKey.None)
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
