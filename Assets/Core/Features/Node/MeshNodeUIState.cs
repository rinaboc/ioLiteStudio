using System.Collections.Generic;
using Core.Domain;
using UnityEngine;

namespace Core.Features
{
    public record MeshNodeUIState(
        int ID,
        string Name,
        int? ParentID,
        int LayerOrder,
        bool Visibility,
        Transform2D DefaultTransform,
        Transform2D LocalTransform,
        MeshInfo DefaultMesh,
        Dictionary<int, Vector2> VertexOffsets
    ) : NodeUIState(ID, Name, ParentID, Visibility, DefaultTransform, LocalTransform);

    public static class MeshNodeExtension
    {
        public static MeshNodeUIState ToMeshNodeUIState(this MeshNode n)
        {
            return new MeshNodeUIState(
                n.Id,
                n.Name,
                n.ParentId,
                n.LayerOrder,
                n.Visibility,
                n.DefaultTransform,
                n.LocalTransform,
                n.DefaultMesh,
                n.VertexOffsets
            );
        }
    }
}
