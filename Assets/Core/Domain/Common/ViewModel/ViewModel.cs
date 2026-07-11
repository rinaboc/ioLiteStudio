using System.Collections.Generic;

namespace Core.Domain
{
    public class ViewModel<TState> : IViewModel<TState>
    {
        protected IStore<TState> _store;
        protected IDispatcher _dispatcher;

        private readonly List<IView<TState>> _views = new();

        public TState State => _store.State;

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
            _store.StateChanged -= OnStateChanged;
            _store.Dispose();
            _store = null;

            foreach (var view in _views.ToArray()) Unbind(view);
            _views.Clear();

            ServiceLocator.Get<ILogService>().LogInfo("[VIEWMODEL] Instance has been disposed.");
        }

        public void Unbind(IView<TState> view)
        {
            if (!_views.Remove(view))
                ServiceLocator.Get<ILogService>().LogError($"[VIEWMODEL] Couldn't unbind view of type {view.GetType()}");

            view.Dispose();
        }
    }
}
