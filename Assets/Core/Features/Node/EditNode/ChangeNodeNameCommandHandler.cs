using Core.Domain;
using UnityEngine;

namespace Core.Features
{
    public class ChangeNodeNameCommandHandler : ICommandHandler<ChangeNodeNameIntent>
    {
        public CommandResult Execute(ChangeNodeNameIntent intent, IAppContext context)
        {
            if (!context.Nodes.TryGet(intent.ID, out var node))
            {
                ServiceLocator.Get<ILogService>().LogError("[CHANGE_NODE_NAME] Couldn't change name of node.");
                return new CommandResult(null, null, null);
            }

            var previousName = node.Name;

            void mutation()
            {
                node.Name = intent.Name;
            }

            void revert()
            {
                node.Name = previousName;
            }

            return new CommandResult(
                mutation,
                revert,
                new ContextMutationResult(ContextMutationType.Node)
            );
        }
    }
}
