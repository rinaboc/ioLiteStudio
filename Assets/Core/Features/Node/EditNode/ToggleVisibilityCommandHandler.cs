using Core.Domain;
using UnityEngine;

namespace Core.Features
{
    public class ToggleVisibilityCommandHandler : ICommandHandler<ToggleVisibilityIntent>
    {
        public CommandResult Execute(ToggleVisibilityIntent intent, IAppContext context)
        {
            if (!context.Nodes.TryGet(intent.ID, out var node))
            {
                ServiceLocator.Get<ILogService>().LogError("[TOGGLE_VISIBILITY] Couldn't change visibility of node.");
                return new CommandResult(null, null, null);
            }

            var previousVisibility = node.Visibility;

            void mutation()
            {
                node.Visibility = !node.Visibility;
            }

            void revert()
            {
                node.Visibility = previousVisibility;
            }

            return new CommandResult(
                mutation,
                revert,
                new ContextMutationResult(ContextMutationType.Node)
            );
        }
    }
}
