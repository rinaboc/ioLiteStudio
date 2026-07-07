namespace Core.Domain
{
    public enum NodeType { Mesh, Deformer }

    public abstract class Node
    {
        public int Id;
        public string Name;
        public int? ParentId;
        public bool Visibility = true;
        public abstract NodeType Type { get; }
        public Transform2D DefaultTransform = new();
        public Transform2D LocalTransform = new();

        protected NodeConfig NodeConfig
        {
            get
            {
                return new NodeConfig(Name, Visibility, DefaultTransform);
            }
            set
            {
                Name = value.Name;
                Visibility = value.Visibility;
                DefaultTransform = value.DefaultTransform;
            }
        }
    }
}
