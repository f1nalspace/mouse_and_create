using System;
using System.Collections.Generic;

namespace MouseAndCreate.Behaviors
{
    public interface IBehaviorManager : IReadOnlyCollection<IBehavior>
    {
        void Add(IBehavior behavior);
        bool Remove(IBehavior behavior);
        void Clear();
        bool Contains(IBehavior behavior);
        bool Contains(Guid id);
        IBehavior GetById(Guid id);
    }
}
