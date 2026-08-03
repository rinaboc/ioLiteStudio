using System.Linq;
using Core.Domain;
using Core.Services;

namespace Core.Features
{
    public class ReparentNodeCommandHandler : ICommandHandler<ReparentNodeIntent>
    {
        public CommandResult Execute(ReparentNodeIntent intent, IAppContext context)
        {
            if (!context.Nodes.TryGet(intent.NodeID, out var node))
            {
                ServiceLocator.Get<ILogService>().LogError("[REPARENT_NODE] Couldn't reparent node.");
                return CommandResult.None;
            }

            if (intent.ParentID != null)
            {
                context.Nodes.TryGet((int)intent.ParentID, out var parent);
                if (parent.Type != NodeType.Deformer)
                {
                    ServiceLocator.Get<ILogService>().LogError("[REPARENT_NODE] Only deformer nodes can be used as parents.");
                    return CommandResult.None;
                }
            }

            if ((intent.ParentID != null && context.Nodes.GetChildGraph(node.Id).Contains((int)intent.ParentID))
                || intent.ParentID == node.Id
            )
            {
                return CommandResult.None;
            }

            var previousParent = node.ParentId;

            void reparentNodeTo(int? parent)
            {
                node.ParentId = parent;
                ServiceLocator.Get<INodeObjectManager>().Reparent(node.Id, parent);
            }

            return new CommandResult(
                () => reparentNodeTo(intent.ParentID),
                () => reparentNodeTo(previousParent),
                new ContextMutationResult(ContextMutationType.Node)
            );
        }
    }
}
