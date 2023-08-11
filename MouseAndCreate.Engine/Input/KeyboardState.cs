using System.Collections;

namespace MouseAndCreate.Input
{
    public class KeyboardState
    {
        private readonly BitArray _keys = new BitArray(256);
        private readonly BitArray _keysPrevious = new BitArray(256);

        public bool this[Key key]
        {
            get
            {
                return IsKeyDown(key);
            }
            private set
            {
                SetKeyState(key, value);
            }
        }

        public KeyboardState()
        {
            _keys = new BitArray(256);
            _keysPrevious = new BitArray(256);
        }

        internal KeyboardState(KeyboardState source)
        {
            _keys = (BitArray)source._keys.Clone();
            _keysPrevious = (BitArray)source._keysPrevious.Clone();
        }

        public bool IsKeyDown(Key key) => _keys[(int)key];
        public bool IsKeyPressed(Key key) => _keys[(int)key] && !_keysPrevious[(int)key];
        public bool IsKeyReleased(Key key) => !_keys[(int)key] && _keysPrevious[(int)key];
        public bool WasKeyDown(Key key) => _keysPrevious[(int)key];
        public void SetKeyState(Key key, bool down) => _keys[(int)key] = down;

        internal void NewFrame()
        {
            _keysPrevious.SetAll(false);
            _keysPrevious.Or(_keys);
        }


        public KeyboardState GetSnapshot() => new KeyboardState(this);
    }
}
