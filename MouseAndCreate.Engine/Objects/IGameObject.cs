using System;
using MouseAndCreate.Types;

namespace MouseAndCreate.Objects;

public interface IGameObject :
    ITranslation,
    IAssignable<IGameObject>,
    ICloneable<IGameObject>,
    IEquatable<IGameObject>,
    IDisposable
{
    Guid Id { get; }
    string Name { get; set; }
}
