namespace Core.Domain
{
    public class HalfEdge
    {
        public Vertex Origin;
        public HalfEdge Next;
        public HalfEdge Twin;
        public Face Face;
        public bool IsConstrained;
    }
}
