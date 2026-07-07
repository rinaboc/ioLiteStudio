using System;

namespace Core.Domain
{
    public interface IDispatcher
    {
        event Action<ContextMutationResult> ContextMutated;
        void Dispatch<TIntent>(TIntent intent) where TIntent : IIntent;
        void CheckResultQueue();
        IAppContext Context { get; }
    }
}