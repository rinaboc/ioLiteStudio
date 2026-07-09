using Core.Domain;

namespace Core.Features
{
    public class MeshNodeStore : Store<MeshNodeUIState>
    {
        private int _nodeID;
        public MeshNodeStore(MeshNodeUIState state, IDispatcher dispatcher, int nodeID) : base(state, dispatcher)
        {
            _nodeID = nodeID;
        }

        protected override ContextMutationType[] ObservedMutations => new ContextMutationType[] { ContextMutationType.Node };

        protected override MeshNodeUIState Build(IAppContext context)
        {
            if (!context.Nodes.TryGet(_nodeID, out var node))
            {
                ServiceLocator.Get<ILogService>().LogError("[MESH_NODE_STORE] Cannot build state from context.");
                return null;
            }

            return ((MeshNode)node).ToMeshNodeUIState();
        }
    }
}
