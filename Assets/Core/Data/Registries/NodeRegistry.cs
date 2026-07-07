using System.Collections.Generic;
using System.Linq;
using Core.Domain;

namespace Core.Data
{
    public class NodeRegistry : Registry<Node, int>, INodeRegistry
    {
        private int _meshCount;
        private int _deformerCount;

        public override void Clear()
        {
            base.Clear();
            _meshCount = 0;
            _deformerCount = 0;
        }

        public override bool Register(int id, Node value)
        {
            bool register = base.Register(id, value);

            if (register)
            {
                _meshCount += value.Type == NodeType.Mesh ? 1 : 0;
                _deformerCount += value.Type == NodeType.Deformer ? 1 : 0;
            }

            return register;
        }

        public override bool Remove(int id, out Node value)
        {
            var remove = base.Remove(id, out value);

            if (remove)
            {
                _meshCount -= value.Type == NodeType.Mesh ? 1 : 0;
                _deformerCount -= value.Type == NodeType.Deformer ? 1 : 0;
            }

            return remove;
        }

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

        public int HighestLayerOrder()
        {
            var max = _meshCount > 0 ? GetAll()
                .Where(node => node.Type == NodeType.Mesh)
                .Max(node => (node as MeshNode).LayerOrder) : 0;
            return max;
        }
    }
}
