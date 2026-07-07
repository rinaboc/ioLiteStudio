using System.Collections.Generic;
using UnityEngine;

namespace Core.Domain
{
    public record MeshNodeConfig(
        NodeConfig NodeConfig,
        string TexPath,
        int? LayerOrder,
        MeshInfo DefaultMesh
    );
}
