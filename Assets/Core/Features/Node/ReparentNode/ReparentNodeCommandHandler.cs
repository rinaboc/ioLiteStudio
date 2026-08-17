using Core.Domain;
using Core.Services;

namespace Core.Features
{
    public class ReparentNodeCommandHandler : ICommandHandler<ReparentNodeIntent>
    {
        public CommandResult Execute(ReparentNodeIntent intent, IAppContext context)
        {
            var logService = ServiceLocator.Get<ILogService>();

            if (!context.Nodes.TryGet(intent.NodeID, out var node))
            {
                logService.LogError("[REPARENT_NODE] Couldn't reparent node.");
                return CommandResult.None;
            }

            if (!ReparentValidatorHelper.TryValidateParent(intent.ParentID, context, logService))
                return CommandResult.None;

            if (ReparentValidatorHelper.IsCircularReparenting(intent.ParentID, node.Id, context))
            {
                logService.LogError("[REPARENT_NODE] New parent creates circular dependency.");
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
