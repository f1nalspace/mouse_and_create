using System;
using System.Collections;

namespace MouseAndCreate.Input
{
    public class GamepadState
    {
        internal const int MaxButtons = 16;

        private readonly BitArray _buttons;
        private readonly BitArray _buttonsPrevious;

        public Guid Id { get; }
        public string Name { get; set; }

        public bool IsConnected { get; internal set; }
        public bool IsPreviousConnected { get; internal set; }

        public bool this[GamepadButton button]
        {
            get => _buttons[(int)button];
            set { _buttons[(int)button] = value; }
        }

        public GamepadState()
        {
            _buttons = new BitArray(MaxButtons);
            _buttonsPrevious = new BitArray(MaxButtons);
            Id = Guid.NewGuid();
            Name = null;
            IsConnected = false;
            IsPreviousConnected = false;
        }

        internal GamepadState(GamepadState source)
        {
            _buttons = (BitArray)source._buttons.Clone();
            _buttonsPrevious = (BitArray)source._buttonsPrevious.Clone();
            Id = source.Id;
            Name = source.Name;
            IsConnected = source.IsConnected;
            IsPreviousConnected = source.IsPreviousConnected;
        }

        public bool IsButtonDown(MouseButton button) => _buttons[(int)button];
        public bool WasButtonDown(MouseButton button) => _buttonsPrevious[(int)button];
        public bool IsButtonPressed(MouseButton button) => _buttons[(int)button] && !_buttonsPrevious[(int)button];
        public bool IsButtonReleased(MouseButton button) => !_buttons[(int)button] && _buttonsPrevious[(int)button];

        internal void NewFrame()
        {
            _buttonsPrevious.SetAll(false);
            _buttonsPrevious.Or(_buttons);

            IsPreviousConnected = IsConnected;
        }

        public GamepadState GetSnapshot() => new GamepadState(this);
    }
}
