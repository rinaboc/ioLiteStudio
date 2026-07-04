using System.Collections.Generic;

namespace Core.Domain
{
    public class MeshInfo
    {
        public List<HalfEdge> HalfEdges = new();
        public List<Vertex> Vertices = new();
        public List<Face> Faces = new();
    }
}
