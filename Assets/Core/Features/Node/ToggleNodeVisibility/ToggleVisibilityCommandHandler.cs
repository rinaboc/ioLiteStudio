using System.Linq;
using Core.Domain;

namespace Core.Features
{
    public class ToggleNodeVisibilityCommandHandler : ICommandHandler<ToggleNodeVisibilityIntent>
    {
        public CommandResult Execute(ToggleNodeVisibilityIntent intent, IAppContext context)
        {
            if (!context.Nodes.TryGet(intent.ID, out var node))
            {
                ServiceLocator.Get<ILogService>().LogError("[TOGGLE_VISIBILITY] Couldn't change visibility of node.");
                return CommandResult.None;
            }

            var previousVisibility = node.Visibility;
            var children = context.Nodes.GetValues(context.Nodes.GetChildGraph(node.Id).ToArray());

            void setVisibility(bool visibility)
            {
                node.Visibility = visibility;
                foreach (var child in children)
                    child.Visibility = visibility;
            }

            return new CommandResult(
                () => setVisibility(!node.Visibility),
                () => setVisibility(previousVisibility),
                new ContextMutationResult(ContextMutationType.Node)
            );
        }
    }
}
