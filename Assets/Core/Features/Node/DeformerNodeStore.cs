using Core.Domain;

namespace Core.Features
{
    public class DeformerNodeStore : Store<DeformerNodeUIState>
    {
        private int _nodeID;
        public DeformerNodeStore(DeformerNodeUIState state, IDispatcher dispatcher, int nodeID) : base(state, dispatcher)
        {
            _nodeID = nodeID;
        }

        protected override ContextMutationType[] ObservedMutations => new ContextMutationType[] { ContextMutationType.Node };

        protected override DeformerNodeUIState Build(IAppContext context)
        {
            if (!context.Nodes.TryGet(_nodeID, out var node))
            {
                ServiceLocator.Get<ILogService>().LogError("[MESH_NODE_STORE] Cannot build state from context.");
                return null;
            }

            return ((DeformerNode)node).ToDeformerNodeUIState();
        }
    }
}
