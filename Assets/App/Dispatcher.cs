using System;
using System.Collections.Concurrent;
using Core.Domain;
using Core.Services;

public class Dispatcher : IDispatcher
{
    private readonly IAppContext _context;
    private readonly IHandlerManager _handlerManager;
    private readonly ConcurrentQueue<(IIntent, CommandResult)> _resultQueue = new();
    public event Action<ContextMutationResult> ContextMutated;
    public IAppContext Context => _context;

    public Dispatcher(IAppContext context, IHandlerManager handlerManager)
    {
        _context = context;
        _handlerManager = handlerManager;
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
        if (_handlerManager.TryGetAsyncHandler<TIntent>(out var asyncHandler))
        {
            ServiceLocator.Get<IThreadPoolService>().Post(async () =>
            {
                var result = await asyncHandler.Execute(intent, _context);
                _resultQueue.Enqueue((intent, result));
            });
            return;
        }
        else if (_handlerManager.TryGetSyncHandler<TIntent>(out var handler))
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
}
