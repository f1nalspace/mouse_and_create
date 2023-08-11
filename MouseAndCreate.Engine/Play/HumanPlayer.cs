using System;

namespace MouseAndCreate.Play;

public class HumanPlayer : IPlayer, IEquatable<HumanPlayer>
{
    public Guid Id { get; }
    public string Name { get; set; }

    public HumanPlayer(Guid id)
    {
        Id = id;
    }

    public override string ToString() => $"{Name} [{Id}]";

    public bool Equals(HumanPlayer other) => other is not null && Id.Equals(other.Id);
    public override bool Equals(object obj) => obj is HumanPlayer player && Equals(player);
    public override int GetHashCode() => Id.GetHashCode();
}
