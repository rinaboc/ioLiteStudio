using System;
using Core.Domain;
using Core.Services;

public class LogDispatcher : IDispatcher
{
    private readonly IDispatcher _dispatcher;

    public event Action<ContextMutationResult> ContextMutated;
    public IAppContext Context => _dispatcher.Context;

    public LogDispatcher(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        _dispatcher.ContextMutated += OnInnerContextMutated;
    }

    private void OnInnerContextMutated(ContextMutationResult result)
    {
        ServiceLocator.Get<ILogService>().LogInfo("[MUTATION SUCCESS] Context changed", result);
        ContextMutated?.Invoke(result);
    }

    public void CheckResultQueue()
    {
        _dispatcher.CheckResultQueue();
    }

    public void Dispatch<TIntent>(TIntent intent) where TIntent : IIntent
    {
        ServiceLocator.Get<ILogService>().LogInfo("[INTENT] Intent received", intent);
        _dispatcher.Dispatch(intent);
    }

    public void RegisterHandler<TIntent>(ICommandHandler<TIntent> handler) where TIntent : IIntent
    {
        _dispatcher.RegisterHandler(handler);
    }

    public void RegisterHandler<TIntent>(IAsyncCommandHandler<TIntent> handler) where TIntent : IIntent
    {
        _dispatcher.RegisterHandler(handler);
    }
}
