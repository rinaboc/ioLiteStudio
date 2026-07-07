using Core.Domain;

namespace Core.Features
{
    public class ReparentNodeCommandHandler : ICommandHandler<ReparentNodeIntent>
    {
        public CommandResult Execute(ReparentNodeIntent intent, IAppContext context)
        {
            if (!context.Nodes.TryGet(intent.NodeID, out var node) || (intent.ParentID != null && !context.Nodes.TryGet((int)intent.ParentID, out _)))
            {
                ServiceLocator.Get<ILogService>().LogError("[REPARENT_NODE] Couldn't reparent node.");
                return new CommandResult(null, null, null);
            }

            var previousParent = node.ParentId;

            void mutation()
            {
                node.ParentId = intent.ParentID;
            }

            void revert()
            {
                node.ParentId = previousParent;
            }

            return new CommandResult(
                mutation,
                revert,
                new ContextMutationResult(ContextMutationType.Node)
            );
        }
    }
}
