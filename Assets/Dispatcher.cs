using System;
using System.Collections.Generic;
using Core.Domain;

public class Dispatcher : IDispatcher
{
    private readonly IAppContext _context;
    private readonly Dictionary<Type, object> _handlers = new();
    private readonly Dictionary<Type, object> _asyncHandlers = new();
    public event Action<ContextMutationResult> ContextMutated;

    public Dispatcher(IAppContext context)
    {
        _context = context;
    }

    public void CheckResultQueue()
    {
        throw new NotImplementedException();
    }

    public void Dispatch<TIntent>(TIntent intent) where TIntent : IIntent
    {
        throw new NotImplementedException();
    }

    public void RegisterHandler<TIntent>(ICommandHandler<TIntent> handler) where TIntent : IIntent
    {
        if (_handlers.ContainsKey(typeof(TIntent)))
        {
            // TODO: dispatch error log
            return;
        }

        _handlers[typeof(TIntent)] = handler;
    }

    public void RegisterHandler<TIntent>(IAsyncCommandHandler<TIntent> handler) where TIntent : IIntent
    {
        if (_asyncHandlers.ContainsKey(typeof(TIntent)))
        {
            // TODO: dispatch error log
            return;
        }

        _asyncHandlers[typeof(TIntent)] = handler;
    }
}
