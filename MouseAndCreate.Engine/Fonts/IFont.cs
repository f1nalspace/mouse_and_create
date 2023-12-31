﻿using MouseAndCreate.Types;
using System;
using System.Collections.Generic;

namespace MouseAndCreate.Fonts;

public interface IFont : IDisposable
{
    IReadOnlyDictionary<int, Glyph> Glyphs { get; }
    float FontSize { get; }
    float LineAdvance { get; }
    float Spacing { get; }
    float Ascent { get; }
    float Descent { get; }
}
