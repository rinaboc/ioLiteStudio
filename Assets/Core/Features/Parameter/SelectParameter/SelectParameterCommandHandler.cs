using Core.Domain;
using UnityEngine;

namespace Core.Features
{
    public class SelectParameterCommandHandler : ICommandHandler<SelectParameterIntent>
    {
        public CommandResult Execute(SelectParameterIntent intent, IAppContext context)
        {
            if (context.CurrentSession.CurrentWorkspace != WorkspaceMode.Default)
                return CommandResult.None;

            var session = context.CurrentSession as DefaultSession;
            var prevSelectedId = session.SelectedParamId;

            void mutation()
            {
                session.SelectedParamId = intent.ID;
            }

            void revert()
            {
                session.SelectedParamId = prevSelectedId;
            }

            return new CommandResult(
                mutation,
                revert,
                new ContextMutationResult(
                    ContextMutationType.Parameter
                )
            );
        }
    }
}
