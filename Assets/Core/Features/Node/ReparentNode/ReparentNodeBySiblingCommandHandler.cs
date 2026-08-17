using Core.Domain;
using Core.Services;

namespace Core.Features
{
    public class ReparentNodeBySiblingCommandHandler : ICommandHandler<ReparentNodeBySiblingIntent>
    {
        public CommandResult Execute(ReparentNodeBySiblingIntent intent, IAppContext context)
        {
            var logger = ServiceLocator.Get<ILogService>();
            if (!context.Nodes.TryGet(intent.NodeID, out var node) || intent.NodeID == intent.SiblingID)
            {
                logger.LogError("[REPARENT_NODE_BY_SIBLING] Couldn't reparent node.");
                return CommandResult.None;
            }

            if (intent.SiblingID is not int sid || !context.Nodes.TryGet(sid, out var sibling))
            {
                logger.LogError("[REPARENT_NODE_BY_SIBLING] Sibling is null");
                return CommandResult.None;
            }

            int? newParentID = sibling.ParentId;

            if (!ReparentValidatorHelper.TryValidateParent(newParentID, context, logger))
                return CommandResult.None;

            if (ReparentValidatorHelper.IsCircularReparenting(newParentID, node.Id, context))
            {
                logger.LogError("[REPARENT_NODE_BY_SIBLING] New parent creates circular dependency.");
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
