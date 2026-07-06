using Core.Domain;

namespace Core.Features
{
    public class SelectNodeCommandHandler : ICommandHandler<SelectNodeIntent>
    {
        public CommandResult Execute(SelectNodeIntent intent, IAppContext context)
        {
            if (context.CurrentSession.CurrentWorkspace != WorkspaceMode.Default)
                return new CommandResult(null, null, null);

            var session = context.CurrentSession as DefaultSession;
            var prevSelectedId = session.SelectedNodeId;

            void mutation()
            {
                session.SelectedNodeId = intent.ID;
            }

            void revert()
            {
                session.SelectedNodeId = prevSelectedId;
            }

            return new CommandResult(
                mutation,
                revert,
                new ContextMutationResult(ContextMutationType.Node)
            );
        }
    }
}
