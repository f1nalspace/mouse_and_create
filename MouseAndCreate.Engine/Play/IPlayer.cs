using System;

namespace MouseAndCreate.Play;

public interface IPlayer
{
    Guid Id { get; }
    string Name { get; }
}
