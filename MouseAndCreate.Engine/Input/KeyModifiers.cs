using System;

namespace MouseAndCreate.Input
{
    [Flags]
    public enum KeyModifiers : byte
    {
        None = 0,
        Shift = 1 << 0,
        Alt = 1 << 1,
        Ctrl = 1 << 2,
    }
}
