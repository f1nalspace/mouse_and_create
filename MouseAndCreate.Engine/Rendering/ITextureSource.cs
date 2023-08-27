﻿using System.IO;

namespace MouseAndCreate.Rendering
{
    public interface ITextureSource
    {
        string Name { get; }
        TextureData Load(TextureFormat target, TextureLoadFlags flags);
    }
}
