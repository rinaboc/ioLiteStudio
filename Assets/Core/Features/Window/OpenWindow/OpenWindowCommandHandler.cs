using Core.Domain;

namespace Core.Features
{
    public class OpenWindowCommandHandler : ICommandHandler<OpenWindowIntent>
    {
        public CommandResult Execute(OpenWindowIntent intent, IAppContext context)
        {
            return CommandResult.MutationOnly(
                () =>
                {
                    context.CurrentSession.IsWindowOpen = true;
                    context.CurrentSession.WindowType = intent.WindowType;
                },
                new ContextMutationResult(ContextMutationType.Session)
            );
        }
    }
}
