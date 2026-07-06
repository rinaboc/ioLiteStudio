using System.Collections.Generic;
using UnityEngine;

namespace Core.Domain
{
    public class MeshNode : Node
    {
        public override NodeType Type => NodeType.Mesh;
        public Texture2D Tex;
        public int LayerOrder;
        public MeshInfo DefaultMesh = new();
        public Dictionary<int, Vector2> VertexOffsets = new();

        public MeshNode() { }

        public MeshNode(MeshNodeConfig meshNodeConfig)
        {
            MeshNodeConfig = meshNodeConfig;
        }

        public MeshNodeConfig MeshNodeConfig
        {
            get
            {
                return new MeshNodeConfig(NodeConfig, Tex, LayerOrder, DefaultMesh);
            }
            set
            {
                NodeConfig = value.NodeConfig;
                Tex = value.Tex;
                LayerOrder = value.LayerOrder;
                DefaultMesh = value.DefaultMesh;
            }
        }
    }
}
