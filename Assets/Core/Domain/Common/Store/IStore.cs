using System;

namespace Core.Domain
{
    public interface IStore<TState>
    {
        event Action<TState> StateChanged;
        void Dispose();
        TState State { get; }
    }
}