using MouseAndCreate.Configurations;
using MouseAndCreate.Objects;
using System;
using System.ComponentModel;

namespace MouseAndCreate.Frames
{
    public interface IFrame : IEquatable<IFrame>, INotifyPropertyChanged, IDisposable
    {
        Guid Id { get; }
        string Name { get; set; }
        FrameSetup Setup { get; }
        IGameObjectManager ObjectMng { get; }
    }
}
