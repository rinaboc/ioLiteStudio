using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Domain
{
    public abstract class ParameterKey
    {
        public Guid Id;
        public int TargetNodeId;
        public Dictionary<Guid, int> ParameterConditions;
        // TODO: public Transform2D TransformDelta;
    }

    public class ParameterMeshKey : ParameterKey
    {
        public Dictionary<int, Vector2> VertexOffsets;
    }

    public class ParameterDeformerKey : ParameterKey
    {

    }
}
