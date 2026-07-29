using System.Collections.Generic;
using System.Linq;
using Core.Domain;

namespace Core.Features
{
    public static class NodeHierarchyExtension
    {
        public static int GetEffectiveLayerOrder(this Node node, IEnumerable<Node> allNodes)
        {
            if (node is MeshNode meshNode) return meshNode.LayerOrder;

            var children = allNodes.Where(n => n.ParentId == node.Id).ToList();

            if (!children.Any()) return -1;

            return children.Max(child => child.GetEffectiveLayerOrder(allNodes));
        }
    }
}