using Core.Domain;

namespace Core.Features
{
    public class CloseWindowCommandHandler : ICommandHandler<CloseWindowIntent>
    {
        public CommandResult Execute(CloseWindowIntent intent, IAppContext context)
        {
            return CommandResult.MutationOnly(
                () => context.CurrentSession.IsWindowOpen = false,
                new ContextMutationResult(ContextMutationType.Session)
            );
        }
    }
}
