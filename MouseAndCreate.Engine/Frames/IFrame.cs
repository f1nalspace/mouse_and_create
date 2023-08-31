using MouseAndCreate.Configurations;
using MouseAndCreate.Objects;
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
