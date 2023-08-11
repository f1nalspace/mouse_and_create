using System;
using MouseAndCreate.Types;

namespace MouseAndCreate.Objects;

public abstract class BackgroundObject : BaseGameObject, IAssignable<BackgroundObject>
{
    public ObstacleType Obstacle { get; set; } = ObstacleType.None;

    public BackgroundObject(Guid? id = null, string name = null) : base(id, name)
    {
    }

    public override void Assign(IGameObject other)
    {
        base.Assign(other);
    }

    public virtual void Assign(BackgroundObject other)
    {
        if (other is null)
            return;
        Assign(other as IGameObject);
        Obstacle = other.Obstacle;
    }
}
