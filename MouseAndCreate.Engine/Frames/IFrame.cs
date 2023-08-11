using MouseAndCreate.Objects;
using MouseAndCreate.Setups;
using System;

namespace MouseAndCreate.Frames
{
    public interface IFrame : IEquatable<IFrame>, IDisposable
    {
        Guid Id { get; }

        string Name { get; set; }

        FrameSetup Setup { get; }

        IGameObjectManager ObjectMng { get; }
    }
}
