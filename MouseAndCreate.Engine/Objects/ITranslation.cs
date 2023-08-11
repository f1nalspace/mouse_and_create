using OpenTK.Mathematics;

namespace MouseAndCreate.Objects;

public interface ITranslation
{
    Vector2 Pos { get; set; }
    Vector2 Scale { get; set; }
    Matrix2 Rotation { get; set; }

    event TranslationChangedEventHandler TranslationChanged;
}

public delegate void TranslationChangedEventHandler(ITranslation translation);
