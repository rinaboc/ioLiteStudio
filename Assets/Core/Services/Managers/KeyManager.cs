using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain;

namespace Core.Services
{
    public interface IKeyManager
    {
        int Next();
        void Restore(IReadOnlyCollection<Node> nodes);
        void Reset();
    }
    public class KeyManager : IKeyManager
    {
        private int _counter = 0;

        public int Next()
        {
            return _counter++;
        }

        public void Reset()
        {
            _counter = 0;
        }

        public void Restore(IReadOnlyCollection<Node> nodes)
        {
            if (nodes.Count == 0)
            {
                ServiceLocator.Get<ILogService>()?
                .LogError("[KEYMANAGER] Cound't restore ID counter.", new IndexOutOfRangeException());
                return;
            }

            var sortedNodes = nodes.OrderByDescending((node) => node.Id).ToArray();
            _counter = sortedNodes[0].Id + 1;
        }
    }
}
