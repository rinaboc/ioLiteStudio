namespace Core.Domain
{
    public interface IViewModel<TState>
    {
        void Dispatch<TIntent>(TIntent intent) where TIntent : IIntent;
        void Bind(IView<TState> view);
        void Unbind(IView<TState> view);
        void Dispose();
    }
}