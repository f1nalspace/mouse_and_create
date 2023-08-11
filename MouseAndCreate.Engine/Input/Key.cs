namespace MouseAndCreate.Input
{
    // Mostly Based on MS Virtual Key Codes, but slightly modified
    public enum Key : byte
    {
        None = 0,

        // 0x0-0x07: Undefined

        Backspace = 0x08,
        Tab = 0x09,

        // 0x0A-0x0B: Reserved

        Clear = 0x0C,
        Enter = 0x0D,

        // 0x0E-0x0F: Undefined

        Shift = 0x10,
        Control = 0x11,
        Alt = 0x12,
        Pause = 0x13,
        CapsLock = 0x14,

        // 0x15: IME-Keys
        // 0x16: Undefined
        // 0x17-0x19 IME-Keys
        // 0x1A: Undefined

        Escape = 0x1B,

        // 0x1C-0x1F: IME-Keys

        Space = 0x20,
        PageUp = 0x21,
        PageDown = 0x22,
        End = 0x23,
        Home = 0x24,
        Left = 0x25,
        Up = 0x26,
        Right = 0x27,
        Down = 0x28,
        Select = 0x29,
        PrintScreen = 0x2A,
        Execute = 0x2B,
        Snapshot = 0x2C,
        Insert = 0x2D,
        Delete = 0x2E,
        Help = 0x2F,

        D0 = 0x30,
        D1 = 0x31,
        D2 = 0x32,
        D3 = 0x33,
        D4 = 0x34,
        D5 = 0x35,
        D6 = 0x36,
        D7 = 0x37,
        D8 = 0x38,
        D9 = 0x39,

        // 0x3A-0x40: Undefined

        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,

        LeftSuper = 0x5B,
        RightSuper = 0x5C,
        Apps = 0x5D,
        WinMenu = 0x5E, // Was reserved, but we use it for windows 'menu' key

        // 0x5F: Sleep,

        NumPad0 = 0x60,
        NumPad1 = 0x61,
        NumPad2 = 0x62,
        NumPad3 = 0x63,
        NumPad4 = 0x64,
        NumPad5 = 0x65,
        NumPad6 = 0x66,
        NumPad7 = 0x67,
        NumPad8 = 0x68,
        NumPad9 = 0x69,
        NumPadMultiply = 0x6A,
        NumPadAdd = 0x6B,
        NumPadSeparator = 0x6C,
        NumPadSubstract = 0x6D,
        NumPadDecimal = 0x6E,
        NumPadDivide = 0x6F,
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,
        F13 = 0x7C,
        F14 = 0x7D,
        F15 = 0x7E,
        F16 = 0x7F,
        F17 = 0x80,
        F18 = 0x81,
        F19 = 0x82,
        F20 = 0x83,
        F21 = 0x84,
        F22 = 0x85,
        F23 = 0x86,
        F24 = 0x87,

        NumPadEnter = 0x88, // Was unassigned, we use it for numpad enter key
        NumPadEqual = 0x89, // Was unassigned, we use it for numpad equal key

        // 8A-8F: Unassigned

        NumLock = 0x90,
        ScrollLock = 0x91,

        // 0x92-9x96: OEM specific
        // 0x97-0x9F: Unassigned

        LeftShift = 0xA0,
        RightShift = 0xA1,
        LeftControl = 0xA2,
        RightControl = 0xA3,
        LeftAlt = 0xA4,
        RightAlt = 0xA5,

        // 0xA6-0xFF: Dont care

        Last = 0xFF
    }
}
