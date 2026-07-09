using System.Collections.Generic;
using UnityEngine;

namespace Core.Domain
{
    public class MeshNode : Node
    {
        public override NodeType Type => NodeType.Mesh;
        public Texture2D Tex;
        private string TexPath;
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
                return new MeshNodeConfig(NodeConfig, TexPath, DefaultMesh, LayerOrder);
            }
            set
            {
                NodeConfig = value.NodeConfig;
                TexPath = value.TexPath;
                LayerOrder = value.LayerOrder ?? -1;
                DefaultMesh = value.DefaultMesh;
                LocalTransform = new()
                {
                    Position = Vector2.zero,
                    Scale = Vector2.zero,
                    Rotation = Quaternion.identity
                };
            }
        }
    }
}
