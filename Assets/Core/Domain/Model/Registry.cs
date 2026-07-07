using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain
{
    public class Registry<T, TId> : IRegistry<T, TId>
    {
        protected ConcurrentDictionary<TId, T> _items = new();

        public virtual bool Register(TId id, T value) => _items.TryAdd(id, value);
        public virtual bool Remove(TId id, out T value) => _items.Remove(id, out value);
        public bool TryGet(TId id, out T value) => _items.TryGetValue(id, out value);
        public IReadOnlyCollection<T> GetValues(TId[] ids) => ids.Where(k => _items.ContainsKey(k)).Select(k => _items[k]).ToArray();
        public IReadOnlyCollection<T> GetAll() => _items.Values.ToArray();
        public int Count() => _items.Count;
        public virtual void Clear() => _items.Clear();
    }
}
