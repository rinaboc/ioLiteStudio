using System.Linq;
using Core.Domain;
using Core.Services;

namespace Core.Features
{
    public class ReparentNodeBySiblingCommandHandler : ICommandHandler<ReparentNodeBySiblingIntent>
    {
        public CommandResult Execute(ReparentNodeBySiblingIntent intent, IAppContext context)
        {
            var logger = ServiceLocator.Get<ILogService>();
            if (!context.Nodes.TryGet(intent.NodeID, out var node))
            {
                logger.LogError("[REPARENT_NODE_BY_SIBLING] Couldn't reparent node.");
                return CommandResult.None;
            }

            int? newParentID = null;

            if (intent.SiblingID != null)
            {
                context.Nodes.TryGet((int)intent.SiblingID, out var sibling);
                newParentID = sibling.ParentId;
            }

            if (intent.SiblingID == intent.NodeID
                || node.ParentId == newParentID
                || node.Id == newParentID
                || (newParentID != null && context.Nodes.GetChildGraph(node.Id).Contains((int)newParentID)))
            {
                return CommandResult.None;
            }

            var previousParentID = node.ParentId;

            void reparentNodeTo(int? parent)
            {
                node.ParentId = parent;
                ServiceLocator.Get<INodeObjectManager>().Reparent(node.Id, parent);
            }

            return new CommandResult(
                () => reparentNodeTo(newParentID),
                () => reparentNodeTo(previousParentID),
                new ContextMutationResult(ContextMutationType.Node)
            );
        }
    }
}
