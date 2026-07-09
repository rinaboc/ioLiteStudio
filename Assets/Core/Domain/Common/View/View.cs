namespace Core.Domain
{
    public abstract class View<TState> : IView<TState>
    {
        protected IViewModel<TState> _viewModel;

        public View(IViewModel<TState> viewModel)
        {
            _viewModel = viewModel;
        }

        public void Dispose()
        {
            _viewModel = null;
        }

        public abstract void Render(TState state);
    }
}
