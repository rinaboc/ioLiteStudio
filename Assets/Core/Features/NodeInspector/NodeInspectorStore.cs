using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain;

namespace Core.Features
{
    public class NodeInspectorStore : Store<NodeInspectorUIState>
    {
        public NodeInspectorStore(NodeInspectorUIState state, IDispatcher dispatcher) : base(state, dispatcher)
        {
        }

        protected override ContextMutationType[] ObservedMutations => new ContextMutationType[] { ContextMutationType.Node, ContextMutationType.Session };

        protected override NodeInspectorUIState Build(IAppContext context)
        {
            var session = context.CurrentSession as DefaultSession;

            var result = new List<NodeInspectorElement>();
            var elements = context.Nodes.GetAll();

            var rootNodes = elements
                .Where(n => n.ParentId == null)
                .OrderByDescending(n => n.GetEffectiveLayerOrder(elements));

            foreach (var root in rootNodes)
            {
                AddNodeAndChildren(root, 0, elements, result, session);
            }

            return new NodeInspectorUIState(
                result.ToArray(),
                session.SelectedNodeId,
                session.OpenedWidgets.Contains(Widget.Node)
            );
        }

        private void AddNodeAndChildren(Node parent, int currentDepth, IReadOnlyCollection<Node> elements, List<NodeInspectorElement> result, DefaultSession session)
        {
            var children = elements
                .Where(n => n.ParentId == parent.Id)
                .OrderByDescending(n => n.GetEffectiveLayerOrder(elements));

            result.Add(new NodeInspectorElement(
                parent.Id,
                parent.Name,
                parent.GetEffectiveLayerOrder(elements),
                parent.ParentId,
                parent.Id == session.SelectedNodeId,
                parent.Visibility,
                currentDepth,
                children.Count() > 0,
                parent.Type == NodeType.Deformer && (parent as DeformerNode).IsOpened,
                parent.Type
            ));

            if (parent.Type == NodeType.Deformer && (parent as DeformerNode).IsOpened)
                foreach (var child in children)
                {
                    AddNodeAndChildren(child, currentDepth + 1, elements, result, session);
                }
        }
    }
}
