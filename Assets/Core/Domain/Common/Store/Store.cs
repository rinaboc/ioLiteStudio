using System;
using System.Linq;

namespace Core.Domain
{
    public abstract class Store<TState> : IStore<TState>
    {
        private TState _state;
        private IDispatcher _dispatcher;

        protected abstract ContextMutationType[] ObservedMutations { get; }

        public event Action<TState> StateChanged;

        protected Store(TState state, IDispatcher dispatcher)
        {
            _state = state;
            _dispatcher = dispatcher;

            _dispatcher.ContextMutated += OnContextMutated;
        }

        protected abstract TState Build(IAppContext context);

        private void OnContextMutated(ContextMutationResult result)
        {
            foreach (var mutation in result.ContextMutations)
            {
                if (ObservedMutations.Contains(mutation))
                {
                    _state = Build(_dispatcher.Context);
                    StateChanged?.Invoke(_state);
                    break;
                }
            }
        }

        public void Dispose()
        {
            _dispatcher.ContextMutated -= OnContextMutated;
            ServiceLocator.Get<ILogService>().LogInfo("[STORE] Instance was disposed.");
        }
    }
}
