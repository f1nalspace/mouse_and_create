using System.Collections.Generic;

namespace MouseAndCreate.Input
{
    public class InputState
    {
        const int MaxGamepadCount = 4;

        public MouseState Mouse { get; }
        public KeyboardState Keyboard { get; }

        private readonly GamepadState[] _gamepads;
        public IReadOnlyCollection<GamepadState> Gamepads => _gamepads;
        public GamepadState Gamepad(int index) => _gamepads[index];

        public InputState()
        {
            Mouse = new MouseState();

            Keyboard = new KeyboardState();

            _gamepads = new GamepadState[MaxGamepadCount];
            for (int i = 0; i < _gamepads.Length; i++)
                _gamepads[i] = new GamepadState();
        }

        private InputState(InputState source)
        {
            Mouse = new MouseState(source.Mouse);
            Keyboard = new KeyboardState(source.Keyboard);

            _gamepads = new GamepadState[MaxGamepadCount];
            for (int i = 0; i < _gamepads.Length; i++)
                _gamepads[i] = new GamepadState(source._gamepads[i]);
        }

        internal void NewFrame(IInputQuery query)
        {
            Mouse.NewFrame(query);

            Keyboard.NewFrame();

            for (int i = 0; i < _gamepads.Length; i++)
                _gamepads[i].NewFrame();
        }

        public InputState GetSnapshot() => new InputState(this);
    }
}
