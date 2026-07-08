using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Core.Domain;

namespace Core.Services
{
    public interface IViewModelManager
    {
        void Create<TState>(
            IStore<TState> store,
            Func<IStore<TState>, IDispatcher, IViewModel<TState>> factory);
        void Create<TState>(
            TState state,
            Func<TState, IDispatcher, IStore<TState>> storeFactory,
            Func<IStore<TState>, IDispatcher, IViewModel<TState>> factory);
        bool TryGet<TState>(out ReadOnlyCollection<IViewModel<TState>> viewModels);
        void Destroy<TState>(IViewModel<TState> viewModel);
    }

    public class ViewModelManager : IViewModelManager
    {
        private readonly IDispatcher _dispatcher;
        private readonly Dictionary<Type, List<object>> _viewModels = new();

        public ViewModelManager(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Create<TState>(IStore<TState> store,
                        Func<IStore<TState>, IDispatcher, IViewModel<TState>> factory)
        {
            var viewModel = factory(store, _dispatcher);

            if (!_viewModels.ContainsKey(typeof(TState)))
                _viewModels[typeof(TState)] = new();

            _viewModels[typeof(TState)].Add(viewModel);
        }

        public void Create<TState>(TState state, Func<TState, IDispatcher, IStore<TState>> storeFactory,
                        Func<IStore<TState>, IDispatcher, IViewModel<TState>> factory)
        {
            var store = storeFactory(state, _dispatcher);
            var viewModel = factory(store, _dispatcher);

            if (!_viewModels.ContainsKey(typeof(TState)))
                _viewModels[typeof(TState)] = new();

            _viewModels[typeof(TState)].Add(viewModel);
        }

        public void Destroy<TState>(IViewModel<TState> viewModel)
        {
            if (_viewModels[typeof(TState)].Remove(viewModel))
            {
                ServiceLocator.Get<ILogService>()
                    .LogInfo($"[VIEWMODEL_MANAGER] Removed a copy of {typeof(TState).Name} ViewModel");
            }
        }

        public bool TryGet<TState>(out ReadOnlyCollection<IViewModel<TState>> viewModels)
        {
            viewModels = default;
            if (!_viewModels.ContainsKey(typeof(TState))) return false;

            viewModels = new ReadOnlyCollection<IViewModel<TState>>
                (_viewModels[typeof(TState)].Cast<IViewModel<TState>>().ToList());

            return true;
        }
    }
}
