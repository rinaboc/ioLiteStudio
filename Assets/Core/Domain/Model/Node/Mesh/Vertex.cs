using System.Numerics;

namespace Core.Domain
{
    public class Vertex
    {
        public int Id;
        public Vector2 Position;
        public Vector2 UV;
        public HalfEdge IncidentEdge;
    }
}
