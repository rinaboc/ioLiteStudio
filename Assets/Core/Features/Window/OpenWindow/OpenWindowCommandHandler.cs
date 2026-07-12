using Core.Domain;

namespace Core.Features
{
    public class OpenWindowCommandHandler : ICommandHandler<OpenWindowIntent>
    {
        public CommandResult Execute(OpenWindowIntent intent, IAppContext context)
        {
            return new CommandResult(
                () =>
                {
                    context.CurrentSession.IsWindowOpen = true;
                    context.CurrentSession.WindowType = intent.WindowType;
                },
                null,
                new ContextMutationResult(ContextMutationType.Session)
            );
        }
    }
}
