using System.Collections.Generic;

namespace Core.Domain
{
    public interface INodeRegistry : IRegistry<Node, int>
    {
        IReadOnlyCollection<int> GetChildGraph(int parentId);
        IReadOnlyCollection<int> GetChildren(int parentId);
        int HighestLayerOrder();
    }
}
