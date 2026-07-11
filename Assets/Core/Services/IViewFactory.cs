using Core.Domain;

namespace Core.Services
{
    public interface IViewFactory
    {
        void CreateMeshNodeView(MeshNode meshNode);
        void DeleteMeshNodeView(int ID);
    }
}
