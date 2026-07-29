using Core.Domain;

namespace Core.Services
{
    public interface IViewFactory
    {
        void CreateMeshNodeView(MeshNode meshNode);
        void CreateDeformerNodeView(DeformerNode deformerNode);
        void DeleteNodeView<TState>(int ID);
        void ClearViews();
    }
}
