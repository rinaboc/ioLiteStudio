using Core.Domain;

namespace Core.Features
{
    public class CloseWindowCommandHandler : ICommandHandler<CloseWindowIntent>
    {
        public CommandResult Execute(CloseWindowIntent intent, IAppContext context)
        {
            return new CommandResult(
                () => context.CurrentSession.IsWindowOpen = false,
                null,
                new ContextMutationResult(ContextMutationType.Session)
            );
        }
    }
}
