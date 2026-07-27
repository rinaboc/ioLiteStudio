using System.Linq;
using Core.Domain;
using Core.Services;

namespace Core.Features
{
    public class DeleteNodeCommandHandler : ICommandHandler<DeleteNodeIntent>
    {
        public CommandResult Execute(DeleteNodeIntent intent, IAppContext context)
        {
            if (context.CurrentSession.CurrentWorkspace != WorkspaceMode.Default || !context.Nodes.TryGet(intent.ID, out var node))
            {
                ServiceLocator.Get<ILogService>()?.LogError("[DELETE_NODE] Cannot delete node.");
                return new CommandResult(null, null, null);
            }

            var session = context.CurrentSession as DefaultSession;
            var prevSelectedNodeId = session?.SelectedNodeId;
            var children = context.Nodes.GetValues(context.Nodes.GetChildren(node.Id).ToArray());

            void mutation()
            {
                foreach (var n in children)
                    n.ParentId = node.ParentId;

                context.Nodes.Remove(node.Id, out _);

                if (session != null && session.SelectedNodeId == node.Id)
                    session.SelectedNodeId = null;

                switch (node.Type)
                {
                    case NodeType.Mesh: ServiceLocator.Get<IViewFactory>().DeleteMeshNodeView(node.Id); break;
                    case NodeType.Deformer: break; // TODO: delete node views
                    default: break;
                }
            }

            void revert()
            {
                foreach (var n in children)
                    n.ParentId = node.Id;

                context.Nodes.Register(node.Id, node);

                if (session != null)
                    session.SelectedNodeId = prevSelectedNodeId;

                switch (node.Type)
                {
                    case NodeType.Mesh: ServiceLocator.Get<IViewFactory>().CreateMeshNodeView(node as MeshNode); break;
                    case NodeType.Deformer: break; // TODO: create node view
                    default: break;
                }
            }

            return new CommandResult(
                mutation,
                revert,
                new ContextMutationResult(ContextMutationType.Node)
            );
        }
    }
}
