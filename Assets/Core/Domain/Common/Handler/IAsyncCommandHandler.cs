using UnityEngine;

namespace Core.Domain
{
    public interface IAsyncCommandHandler<TIntent> where TIntent : IIntent
    {
        Awaitable<CommandResult> Execute(TIntent intent, IAppContext context);
    }
}