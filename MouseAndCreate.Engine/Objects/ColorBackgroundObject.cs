using MouseAndCreate.Types;
using OpenTK.Mathematics;
using System;

namespace MouseAndCreate.Objects;

public class ColorBackgroundObject : BackgroundObject, IAssignable<ColorBackgroundObject>
{
    public Color4 Color { get; set; } = new Color4(0, 0, 0, 1);

    public ColorBackgroundObject(Guid? id = null, string name = null) : base(id, name)
    {
    }

    public void Assign(ColorBackgroundObject other)
    {
        base.Assign(other as BackgroundObject);
        if (other is null)
            return;
        Color = other.Color;
    }

    public override IGameObject Clone()
    {
        ColorBackgroundObject result = new ColorBackgroundObject();
        result.Assign(this);
        return result;
    }
}
