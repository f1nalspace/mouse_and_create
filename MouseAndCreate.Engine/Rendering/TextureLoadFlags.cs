using System;

namespace MouseAndCreate.Rendering
{
    [Flags]
    public enum TextureLoadFlags
    {
        None = 0,
        FlipY = 1 << 0
    }
}
