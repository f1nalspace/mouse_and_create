using System;
using MouseAndCreate.Behaviors;
using MouseAndCreate.Play;
using MouseAndCreate.Types;

namespace MouseAndCreate.Objects;

public class ActiveObject : BaseGameObject, IAssignable<ActiveObject>
{
    public IMovement Movement { get; set; } = null;
    public IPlayer Player { get; set; } = null;

    public ActiveObject(Guid? id = null, string name = null) : base(id, name)
    {
    }

    public void Assign(ActiveObject other)
    {
        if (other is null)
            return;
        base.Assign(other as IGameObject);
        Movement = other.Movement?.Clone();
        Player = other.Player;
    }

    public override IGameObject Clone()
    {
        ActiveObject result = new ActiveObject();
        result.Assign(this);
        return result;
    }
}
