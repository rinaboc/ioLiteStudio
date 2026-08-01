using Core.Domain;
using Core.Services;

namespace Core.Features
{
    public class CreateMeshNodeCommandHandler : ICommandHandler<CreateMeshNodeIntent>
    {
        public CommandResult Execute(CreateMeshNodeIntent intent, IAppContext context)
        {
            var keyManager = ServiceLocator.Get<IKeyManager>();
            var texture = ServiceLocator.Get<IImageService>().Load(intent.MeshNodeConfig.TexPath);

            if (keyManager == null || texture == null ||
                context.CurrentSession.CurrentWorkspace != WorkspaceMode.Default)
            {
                ServiceLocator.Get<ILogService>()?.LogError("[CREATE_MESH_NODE] Couldn't create node.");
                return CommandResult.None;
            }

            var session = context.CurrentSession as DefaultSession;

            var meshNode = new MeshNode(intent.MeshNodeConfig)
            {
                Id = keyManager.Next(),
                LayerOrder = context.Nodes.HighestLayerOrder() + 1,
                Tex = texture
            };

            if (session.SelectedNodeId != null && context.Nodes.TryGet((int)session.SelectedNodeId, out var selectedNode))
                meshNode.ParentId = selectedNode.Type == NodeType.Deformer ? selectedNode.Id : null;

            void mutation()
            {
                context.Nodes.Register(meshNode.Id, meshNode);
                ServiceLocator.Get<IViewFactory>().CreateMeshNodeView(meshNode);
            }

            void revert()
            {
                context.Nodes.Remove(meshNode.Id, out _);
                ServiceLocator.Get<IViewFactory>().DeleteNodeView<MeshNodeUIState>(meshNode.Id);
            }

            return new CommandResult(
                mutation,
                revert,
                new ContextMutationResult(ContextMutationType.Node)
            );
        }
    }
}