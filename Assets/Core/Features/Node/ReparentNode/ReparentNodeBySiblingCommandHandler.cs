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

            if (intent.SiblingID == intent.NodeID)
            {
                logger.LogInfo("sibling and node id is the same");
                return CommandResult.None;
            }

            if (node.ParentId == newParentID) return CommandResult.None;

            if (node.Id == newParentID)
            {
                logger.LogInfo("node cannot be parent of itself");
                return CommandResult.None;
            }

            if (newParentID != null &&
                context.Nodes.GetChildGraph(node.Id).Contains((int)newParentID))
            {
                logger.LogError("node's subtree contains the new parent");
                return CommandResult.None;
            }

            var previousParentID = node.ParentId;

            void mutation()
            {
                node.ParentId = newParentID;
                ServiceLocator.Get<INodeObjectManager>().Reparent(node.Id, newParentID);
            }

            void revert()
            {
                node.ParentId = previousParentID;
                ServiceLocator.Get<INodeObjectManager>().Reparent(node.Id, previousParentID);
            }

            return new CommandResult(
                mutation,
                revert,
                new ContextMutationResult(ContextMutationType.Node)
            );
        }
    }
}
