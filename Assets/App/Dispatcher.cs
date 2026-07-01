using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Core.Domain;
using Core.Services;

public class Dispatcher : IDispatcher
{
    private readonly IAppContext _context;
    private readonly Dictionary<Type, object> _handlers = new();
    private readonly Dictionary<Type, object> _asyncHandlers = new();
    private readonly ConcurrentQueue<(IIntent, CommandResult)> _resultQueue = new();
    public event Action<ContextMutationResult> ContextMutated;
    public IAppContext Context => _context;

    public Dispatcher(IAppContext context)
    {
        _context = context;
    }

    public void CheckResultQueue()
    {
        while (_resultQueue.TryDequeue(out (IIntent intent, CommandResult result) item))
        {
            ProcessResult(item.intent, item.result);
        }
    }

    public void Dispatch<TIntent>(TIntent intent) where TIntent : IIntent
    {
        if (TryGetAsyncHandler<TIntent>(out var asyncHandler))
        {
            ServiceLocator.Get<IThreadPoolService>()?.Post(async () =>
            {
                var result = await asyncHandler.Execute(intent, _context);
                _resultQueue.Enqueue((intent, result));
            });
            return;
        }
        else if (TryGetHandler<TIntent>(out var handler))
        {
            var result = handler.Execute(intent, _context);
            ProcessResult(intent, result);
            return;
        }

        ServiceLocator.Get<ILogService>()
            .LogError($"[DISPATCHER] Couldn't find a handler to proccess intent: {typeof(TIntent).Name}");
    }

    private void ProcessResult(IIntent intent, CommandResult result)
    {
        result.Mutation?.Invoke();

        var undoService = ServiceLocator.Get<IUndoService>();
        if (result.Revert != null && undoService != null)
            undoService.PushStep(new HistoryStep(intent, result));

        if (undoService != null)
        {
            Context.CurrentSession.UndoCount = undoService.UndoCount;
            Context.CurrentSession.RedoCount = undoService.RedoCount;
        }

        if (result.MutationResult != null)
            ContextMutated?.Invoke(result.MutationResult);
    }

    public void RegisterHandler<TIntent>(ICommandHandler<TIntent> handler) where TIntent : IIntent
    {
        if (_handlers.ContainsKey(typeof(TIntent)))
        {
            ServiceLocator.Get<ILogService>()?.LogWarning($"[DISPATCHER] Handler of {typeof(TIntent)} already registered.");
            return;
        }

        _handlers[typeof(TIntent)] = handler;
    }

    public void RegisterHandler<TIntent>(IAsyncCommandHandler<TIntent> handler) where TIntent : IIntent
    {
        if (_asyncHandlers.ContainsKey(typeof(TIntent)))
        {
            ServiceLocator.Get<ILogService>()?.LogWarning($"[DISPATCHER] Handler of {typeof(TIntent)} already registered.");
            return;
        }

        _asyncHandlers[typeof(TIntent)] = handler;
    }

    private bool TryGetAsyncHandler<TIntent>(out IAsyncCommandHandler<TIntent> asyncHandler) where TIntent : IIntent
    {
        asyncHandler = null;
        if (_asyncHandlers.TryGetValue(typeof(TIntent), out var handler))
        {
            asyncHandler = (IAsyncCommandHandler<TIntent>)handler;
            return true;
        }

        return false;
    }

    private bool TryGetHandler<TIntent>(out ICommandHandler<TIntent> syncHandler) where TIntent : IIntent
    {
        syncHandler = null;
        if (_handlers.TryGetValue(typeof(TIntent), out var handler))
        {
            syncHandler = (ICommandHandler<TIntent>)handler;
            return true;
        }

        return false;
    }
}
