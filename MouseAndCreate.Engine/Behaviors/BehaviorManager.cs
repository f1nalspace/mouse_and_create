using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MouseAndCreate.Behaviors
{
    class BehaviorManager : IBehaviorManager
    {
        private readonly List<IBehavior> _items = new List<IBehavior>();
        private readonly ConcurrentDictionary<Guid, IBehavior> _map = new ConcurrentDictionary<Guid, IBehavior>();

        public int Count => throw new NotImplementedException();

        public void Add(IBehavior behavior)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(IBehavior behavior)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Guid id)
        {
            throw new NotImplementedException();
        }

        public IBehavior GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IBehavior> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(IBehavior behavior)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
