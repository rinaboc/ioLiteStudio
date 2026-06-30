using System.Collections.Generic;

namespace Core.Services
{
    public class DropOutStack<T>
    {
        private readonly int _capacity;
        private readonly LinkedList<T> _items = new();

        public DropOutStack(int capacity)
        {
            _capacity = capacity;
        }

        public void Push(T item)
        {
            if (_items.Count >= _capacity)
                _items.RemoveLast();

            _items.AddFirst(item);
        }

        public bool TryPop(out T item)
        {
            item = default;
            if (_items.Count == 0)
                return false;

            item = _items.First.Value;
            _items.RemoveFirst();
            return true;
        }

        public T Peek() => _items.First.Value;
        public int Count => _items.Count;
        public void Clear() => _items.Clear();
    }
}
