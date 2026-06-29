using System;
using Core.Domain;

namespace Core.Domain
{
    public interface IDispatcher
    {
        event Action<ContextMutationResult> ContextMutated;
        void RegisterHandler<TIntent>(ICommandHandler<TIntent> handler) where TIntent : IIntent;
        void RegisterHandler<TIntent>(IAsyncCommandHandler<TIntent> handler) where TIntent : IIntent;
        void Dispatch<TIntent>(TIntent intent) where TIntent : IIntent;
        void CheckResultQueue();
    }
}