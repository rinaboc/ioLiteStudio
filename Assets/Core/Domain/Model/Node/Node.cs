using System.Collections.Generic;
using UnityEngine;

namespace Core.Domain
{
    public enum NodeType { Mesh, Deformer }

    public abstract class Node
    {
        public int Id;
        public string Name;
        public int? ParentId;
        public abstract NodeType Type { get; }
        // public Transform2D DefaultTransform;
        // public Transform2D LocalTransform;
    }

    public class DeformerNode : Node
    {
        public override NodeType Type => NodeType.Deformer;
    }

    public class MeshNode : Node
    {
        public override NodeType Type => NodeType.Mesh;
        public Texture2D Tex;
        public int LayerOrder;
        // public MeshInfo DefaultMesh;
        public Dictionary<int, Vector2> VertexOffsets;
    }
}
