namespace Core.Domain
{
    public class DeformerNode : Node
    {
        public override NodeType Type => NodeType.Deformer;

        public DeformerNode() { }

        public DeformerNode(DeformerNodeConfig deformerNodeConfig)
        {
            DeformerNodeConfig = deformerNodeConfig;
        }

        public DeformerNodeConfig DeformerNodeConfig
        {
            get
            {
                return new DeformerNodeConfig(NodeConfig);
            }
            set
            {
                NodeConfig = value.NodeConfig;
            }
        }
    }
}
