using System;

namespace MouseAndCreate.Types
{
    public interface IResource : IDisposable
    {
        Guid Id { get; }
        string Name { get; }
        bool IsDisposed { get; }
    }
}
