using System.Collections;

namespace MouseAndCreate.Input
{
    public class KeysStates
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

        public bool IsKeyDown(Key key) => _keys[(int)key];
        public bool IsKeyPressed(Key key) => _keys[(int)key] && !_keysPrevious[(int)key];
        public bool IsKeyReleased(Key key) => !_keys[(int)key] && _keysPrevious[(int)key];
        public bool WasKeyDown(Key key) => _keysPrevious[(int)key];
        public void SetKeyState(Key key, bool down) => _keys[(int)key] = down;

        public KeysStates()
        {
            _keys = new BitArray(256);
            _keysPrevious = new BitArray(256);
        }

        private KeysStates(KeysStates source)
        {
            _keys = (BitArray)source._keys.Clone();
            _keysPrevious = (BitArray)source._keysPrevious.Clone();
        }

        public KeysStates GetSnapshot() => new KeysStates(this);
    }
}
