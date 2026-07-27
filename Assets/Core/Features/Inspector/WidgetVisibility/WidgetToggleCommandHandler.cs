using Core.Domain;

namespace Core.Features
{
    public class WidgetToggleCommandHandler : ICommandHandler<WidgetToggleIntent>
    {
        public CommandResult Execute(WidgetToggleIntent intent, IAppContext context)
        {
            if (context.CurrentSession is not DefaultSession) return new CommandResult(null, null, null);

            DefaultSession session = context.CurrentSession as DefaultSession;

            void mutation()
            {
                if (intent.IsOpen && !session.OpenedWidgets.Contains(intent.Widget))
                    session.OpenedWidgets.Add(intent.Widget);
                else
                    session.OpenedWidgets.Remove(intent.Widget);

            }

            return new CommandResult(
                mutation,
                null,
                new ContextMutationResult(ContextMutationType.Session)
            );
        }
    }
}
