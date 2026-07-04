using System.Collections.Generic;

namespace Core.Domain
{
    public interface IRegistry<T, TId>
    {
        bool Register(TId id, T value);
        bool Remove(TId id, out T value);
        bool TryGet(TId id, out T value);
        IReadOnlyCollection<T> GetValues(TId[] ids);
        IReadOnlyCollection<T> GetAll();
        int Count();
        void Clear();
    }
}
