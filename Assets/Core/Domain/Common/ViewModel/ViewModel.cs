using System;
using System.Collections.Generic;

namespace Core.Domain
{
    public abstract class ViewModel<TState> : IViewModel<TState>, IDisposable
    {
        protected IStore<TState> _store;
        protected IDispatcher _dispatcher;

        private readonly List<IView<TState>> _views = new();

        public ViewModel(IStore<TState> store, IDispatcher dispatcher)
        {
            _store = store;
            _dispatcher = dispatcher;

            _store.StateChanged += OnStateChanged;
        }

        private void OnStateChanged(TState state)
        {
            foreach (var view in _views)
            {
                view.Render(state);
            }
        }

        public void Bind(IView<TState> view)
        {
            if (_views.Contains(view))
            {
                ServiceLocator.Get<ILogService>().LogError("[VIEWMODEL] View is already bound");
                return;
            }

            _views.Add(view);
        }

        public void Dispatch<TIntent>(TIntent intent) where TIntent : IIntent
        {
            _dispatcher.Dispatch(intent);
        }

        public void Dispose()
        {
            _views.Clear();
            _store.StateChanged -= OnStateChanged;
        }

        public void Unbind(IView<TState> view)
        {
            if (!_views.Remove(view))
                ServiceLocator.Get<ILogService>().LogError($"[VIEWMODEL] Coundn't unbind view of type {view.GetType()}");

        }
    }
}
