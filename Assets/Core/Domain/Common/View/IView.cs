namespace Core.Domain
{
    public interface IView<TState>
    {
        void Render(TState state);
    }
}