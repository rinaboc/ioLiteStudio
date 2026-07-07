using Core.Domain;

namespace Core.Features
{
    public record CreateMeshNodeIntent(MeshNodeConfig MeshNodeConfig) : IIntent;
}