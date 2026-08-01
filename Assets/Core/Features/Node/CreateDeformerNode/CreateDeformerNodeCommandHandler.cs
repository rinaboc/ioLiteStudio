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
                return CommandResult.None;
            }

            var session = context.CurrentSession as DefaultSession;

            var deformerNode = new DeformerNode(intent.DeformerNodeConfig)
            {
                Id = keyManager.Next(),
            };

            if (session.SelectedNodeId != null && context.Nodes.TryGet((int)session.SelectedNodeId, out var selectedNode))
                deformerNode.ParentId = selectedNode.Type == NodeType.Deformer ? selectedNode.Id : null;

            void mutation()
            {
                context.Nodes.Register(deformerNode.Id, deformerNode);
                ServiceLocator.Get<IViewFactory>().CreateDeformerNodeView(deformerNode);
            }

            void revert()
            {
                context.Nodes.Remove(deformerNode.Id, out _);
                ServiceLocator.Get<IViewFactory>().DeleteNodeView<DeformerNodeUIState>(deformerNode.Id);
            }

            return new CommandResult(
                mutation,
                revert,
                new ContextMutationResult(ContextMutationType.Node)
            );
        }
    }
}
