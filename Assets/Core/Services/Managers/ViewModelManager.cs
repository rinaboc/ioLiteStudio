using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Core.Domain;

namespace Core.Services
{
    public interface IViewModelManager
    {
        IViewModel<TState> Create<TState>(
            TState state,
            Func<TState, IDispatcher, IStore<TState>> storeFactory);
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

        public IViewModel<TState> Create<TState>(TState state, Func<TState, IDispatcher, IStore<TState>> storeFactory)
        {
            var store = storeFactory(state, _dispatcher);
            var viewModel = new ViewModel<TState>(store, _dispatcher);

            if (!_viewModels.ContainsKey(typeof(TState)))
                _viewModels[typeof(TState)] = new();

            _viewModels[typeof(TState)].Add(viewModel);
            return viewModel;
        }

        public void Destroy<TState>(IViewModel<TState> viewModel)
        {
            if (_viewModels[typeof(TState)].Remove(viewModel))
            {
                viewModel.Dispose();

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
