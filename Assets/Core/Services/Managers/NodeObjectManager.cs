using System.Collections.Generic;
using Core.Domain;
using NUnit.Framework;
using UnityEngine;

namespace Core.Services
{
    public interface INodeObjectManager
    {
        bool Instantiate(MeshNode meshNode, out GameObject meshObject);
        bool Instantiate(DeformerNode deformerNode, out GameObject deformerObject);
        void Destroy(params int[] IDs);
        void Reparent(int node, int? newParent);
        void Configure(Material meshMaterial, Transform sceneRoot);
    }

    public class NodeObjectManager : MonoBehaviour, INodeObjectManager
    {
        [SerializeField] private Material _meshMaterial;
        [SerializeField] private Transform _sceneRoot;

        private Dictionary<int, GameObject> _nodeObjects = new();

        public void Configure(Material meshMaterial, Transform sceneRoot)
        {
            _meshMaterial = meshMaterial;
            _sceneRoot = sceneRoot;
        }

        void Awake()
        {
            Assert.IsNotNull(_meshMaterial);
            Assert.IsNotNull(_sceneRoot);
        }

        public void Destroy(params int[] IDs)
        {
            foreach (var id in IDs)
            {
                if (!_nodeObjects.TryGetValue(id, out var nodeObject)) continue;

                _nodeObjects.Remove(id);
#if UNITY_EDITOR
                DestroyImmediate(nodeObject);
#else
                Destroy(nodeObject);
#endif
            }
        }

        public bool Instantiate(MeshNode meshNode, out GameObject meshObject)
        {
            meshObject = null;
            if (_nodeObjects.ContainsKey(meshNode.Id)) return false;

            var tex = meshNode.Tex;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), pixelsPerUnit: 100, extrude: 1, meshType: SpriteMeshType.Tight);

            Mesh mesh = new()
            {
                vertices = System.Array.ConvertAll(sprite.vertices, v => (Vector3)v),
                triangles = System.Array.ConvertAll(sprite.triangles, t => (int)t),
                uv = sprite.uv
            };

            meshObject = new("MeshObject")
            {
                layer = LayerMask.NameToLayer("UI")
            };

            meshObject.AddComponent<MeshFilter>().mesh = mesh;
            Material _mat = new(_meshMaterial)
            {
                mainTexture = sprite.texture
            };
            meshObject.AddComponent<MeshRenderer>().material = _mat;

            _nodeObjects[meshNode.Id] = meshObject;

            if (meshNode.ParentId == null)
            {
                meshObject.transform.parent = _sceneRoot;
                return true;
            }

            if (_nodeObjects.TryGetValue((int)meshNode.ParentId, out var parentObject))
            {
                meshObject.transform.parent = parentObject.transform;
            }

            return true;
        }

        public bool Instantiate(DeformerNode deformerNode, out GameObject deformerObject)
        {
            deformerObject = null;
            if (_nodeObjects.ContainsKey(deformerNode.Id)) return false;

            deformerObject = new("DeformerObject")
            {
                layer = LayerMask.NameToLayer("UI")
            };

            _nodeObjects[deformerNode.Id] = deformerObject;
            if (deformerNode.ParentId == null)
            {
                deformerObject.transform.parent = _sceneRoot;
                return true;
            }

            if (_nodeObjects.TryGetValue((int)deformerNode.ParentId, out var parentObject))
            {
                deformerObject.transform.parent = parentObject.transform;
            }

            return true;
        }

        public void Reparent(int node, int? newParent)
        {
            if (!_nodeObjects.TryGetValue(node, out var nodeObject))
                return;

            if (newParent == null)
            {
                nodeObject.transform.parent = _sceneRoot;
                return;
            }

            if (!_nodeObjects.TryGetValue((int)newParent, out var parentObject))
                return;

            nodeObject.transform.parent = parentObject.transform;
        }
    }
}
