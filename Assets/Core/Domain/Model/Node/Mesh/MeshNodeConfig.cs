namespace Core.Domain
{
    public record MeshNodeConfig(
        NodeConfig NodeConfig,
        string TexPath,
        MeshInfo DefaultMesh,
        int? LayerOrder = null
    );
}
