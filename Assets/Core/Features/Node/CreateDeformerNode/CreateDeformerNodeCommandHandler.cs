using Core.Domain;
using Core.Services;

namespace Core.Features
{
    public class CreateDeformerNodeCommandHandler : ICommandHandler<CreateDeformerNodeIntent>
    {
        public CommandResult Execute(CreateDeformerNodeIntent intent, IAppContext context)
        {
            var keyManager = ServiceLocator.Get<IKeyManager>();

            if (keyManager == null || context.CurrentSession.CurrentWorkspace != WorkspaceMode.Default)
            {
                ServiceLocator.Get<ILogService>()?.LogError("[CREATE_DEFORMER_NODE] Couldn't create node.");
                return new CommandResult(null, null, null);
            }

            var deformerNode = new DeformerNode(intent.DeformerNodeConfig)
            {
                Id = keyManager.Next(),
                ParentId = (context.CurrentSession as DefaultSession).SelectedNodeId
            };

            void mutation()
            {
                context.Nodes.Register(deformerNode.Id, deformerNode);
            }

            void revert()
            {
                context.Nodes.Remove(deformerNode.Id, out _);
            }

            return new CommandResult(
                mutation,
                revert,
                new ContextMutationResult(ContextMutationType.Node)
            );
        }
    }
}
