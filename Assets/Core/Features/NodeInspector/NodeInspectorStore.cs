using System.Linq;
using Core.Domain;

namespace Core.Features
{
    public class NodeInspectorStore : Store<NodeInspectorUIState>
    {
        public NodeInspectorStore(NodeInspectorUIState state, IDispatcher dispatcher) : base(state, dispatcher)
        {
        }

        protected override ContextMutationType[] ObservedMutations => new ContextMutationType[] { ContextMutationType.Node };

        protected override NodeInspectorUIState Build(IAppContext context)
        {
            var elements = context.Nodes.GetAll()
            .Select(node => new NodeInspectorElement(
                node.Id,
                node.Name,
                node is MeshNode meshNode ? meshNode.LayerOrder : -1,
                node.ParentId
            ));

            var session = context.CurrentSession as DefaultSession;

            return new NodeInspectorUIState(
                elements.ToArray(),
                session.SelectedNodeId,
                session.OpenedWidgets.Contains(Widget.Node)
            );
        }
    }
}
