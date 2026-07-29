namespace Core.Domain
{
    public class DeformerNode : Node
    {
        public override NodeType Type => NodeType.Deformer;
        public bool IsOpened;

        public DeformerNode() { }

        public DeformerNode(DeformerNodeConfig deformerNodeConfig)
        {
            DeformerNodeConfig = deformerNodeConfig;
        }

        public DeformerNodeConfig DeformerNodeConfig
        {
            get
            {
                return new DeformerNodeConfig(NodeConfig, IsOpened);
            }
            set
            {
                NodeConfig = value.NodeConfig;
                IsOpened = value.IsOpened;
            }
        }
    }
}
