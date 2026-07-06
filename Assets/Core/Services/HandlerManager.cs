using System;
using System.Collections.Generic;
using Core.Domain;

namespace Core.Services
{
    public class HandlerManager : IHandlerManager
    {
        private readonly Dictionary<Type, object> _handlers = new();
        private readonly Dictionary<Type, object> _asyncHandlers = new();

        public void Clear()
        {
            _handlers.Clear();
            _asyncHandlers.Clear();
        }

        public void Register<TIntent>(ICommandHandler<TIntent> handler) where TIntent : IIntent
        {
            if (_handlers.ContainsKey(typeof(TIntent)))
            {
                ServiceLocator.Get<ILogService>()?.LogWarning($"[DISPATCHER] Handler of {typeof(TIntent)} already registered.");
                return;
            }

            _handlers[typeof(TIntent)] = handler;
        }

        public void Register<TIntent>(IAsyncCommandHandler<TIntent> handler) where TIntent : IIntent
        {
            if (_asyncHandlers.ContainsKey(typeof(TIntent)))
            {
                ServiceLocator.Get<ILogService>()?.LogWarning($"[DISPATCHER] Handler of {typeof(TIntent)} already registered.");
                return;
            }

            _asyncHandlers[typeof(TIntent)] = handler;
        }

        public bool TryGetAsyncHandler<TIntent>(out IAsyncCommandHandler<TIntent> asyncHandler) where TIntent : IIntent
        {
            asyncHandler = null;
            if (_asyncHandlers.TryGetValue(typeof(TIntent), out var handler))
            {
                asyncHandler = (IAsyncCommandHandler<TIntent>)handler;
                return true;
            }

            return false;
        }

        public bool TryGetSyncHandler<TIntent>(out ICommandHandler<TIntent> syncHandler) where TIntent : IIntent
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
}
