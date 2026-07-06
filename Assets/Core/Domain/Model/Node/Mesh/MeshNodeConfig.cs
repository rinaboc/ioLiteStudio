using System.Collections.Generic;
using UnityEngine;

namespace Core.Domain
{
    public record MeshNodeConfig(
        NodeConfig NodeConfig,
        Texture2D Tex,
        int LayerOrder,
        MeshInfo DefaultMesh
    );
}
