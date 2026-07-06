using System.Collections.Generic;
using Core.Domain;

namespace Core.Data
{
    public class NodeRegistry : Registry<Node, int>, INodeRegistry
    {
        public IReadOnlyCollection<int> GetChildren(int parentID)
        {
            List<int> nodes = new();

            foreach (var node in GetAll())
            {
                if (node.ParentId != null && node.ParentId == parentID)
                    nodes.Add(node.Id);
            }

            return nodes.ToArray();
        }
        public IReadOnlyCollection<int> GetChildGraph(int parentId)
        {
            List<int> children = new();

            var currentNodes = new Queue<int>();
            currentNodes.Enqueue(parentId);
            do
            {
                int currentId = currentNodes.Dequeue();
                children.Add(currentId);
                var currentChildren = GetChildren(currentId);
                foreach (var id in currentChildren) currentNodes.Enqueue(id);
            } while (currentNodes.Count > 0);

            return children;
        }
    }
}
