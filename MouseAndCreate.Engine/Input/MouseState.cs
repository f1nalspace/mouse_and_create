﻿using OpenTK.Mathematics;
using System.Collections;

namespace MouseAndCreate.Input
{
    public class MouseState
    {
        internal const int MaxButtons = 16;

        private readonly BitArray _buttons;
        private readonly BitArray _buttonsPrevious;

        public Vector2 Position { get; set; }
        public Vector2 PreviousPosition { get; private set; }

        public Vector2 Scroll { get; set; }
        public Vector2 PreviousScroll { get; private set; }

        public bool this[MouseButton button]
        {
            get => _buttons[(int)button];
            set { _buttons[(int)button] = value; }
        }

        public MouseState()
        {
            _buttons = new BitArray(MaxButtons);
            _buttonsPrevious = new BitArray(MaxButtons);
        }

        internal MouseState(MouseState source)
        {
            Position = source.Position;
            PreviousPosition = source.PreviousPosition;

            Scroll = source.Scroll;
            PreviousScroll = source.PreviousScroll;

            _buttons = (BitArray)source._buttons.Clone();
            _buttonsPrevious = (BitArray)source._buttonsPrevious.Clone();
        }

        public bool IsButtonDown(MouseButton button) => _buttons[(int)button];
        public bool WasButtonDown(MouseButton button) => _buttonsPrevious[(int)button];
        public bool IsButtonPressed(MouseButton button) => _buttons[(int)button] && !_buttonsPrevious[(int)button];
        public bool IsButtonReleased(MouseButton button) => !_buttons[(int)button] && _buttonsPrevious[(int)button];

        internal void NewFrame(IMouseInputQuery query)
        {
            _buttonsPrevious.SetAll(false);
            _buttonsPrevious.Or(_buttons);

            PreviousPosition = Position;
            PreviousScroll = Scroll;

            Position = query.GetMousePosition();
        }

        public MouseState GetSnapshot() => new MouseState(this);
    }
}
