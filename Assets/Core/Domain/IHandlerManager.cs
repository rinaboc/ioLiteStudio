namespace Core.Domain
{
    public interface IHandlerManager
    {
        void Register<T>(ICommandHandler<T> handler) where T : IIntent;
        void Register<T>(IAsyncCommandHandler<T> handler) where T : IIntent;
        bool TryGetSyncHandler<T>(out ICommandHandler<T> handler) where T : IIntent;
        bool TryGetAsyncHandler<T>(out IAsyncCommandHandler<T> handler) where T : IIntent;
        void Clear();
    }
}
