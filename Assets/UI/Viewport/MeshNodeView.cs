using Core.Domain;
using UnityEngine;

namespace Core.Features
{
    public class MeshNodeView : View<MeshNodeUIState>
    {
        private GameObject _meshObject;
        private Material _meshMaterial;
        public MeshNodeView(IViewModel<MeshNodeUIState> viewModel, GameObject meshObject) : base(viewModel)
        {
            _meshObject = meshObject;
            _meshMaterial = _meshObject.GetComponent<MeshRenderer>().material;
        }

        public override void Render(MeshNodeUIState state)
        {
            var transform = state.DefaultTransform + state.LocalTransform;
            _meshObject.transform.position = new Vector3(transform.Position.x, transform.Position.y, 0f);
            _meshObject.transform.localScale = new Vector3(transform.Scale.y, transform.Scale.y, 0f);
            _meshObject.transform.rotation = transform.Rotation;
            _meshObject.SetActive(state.Visibility);

            if (_meshMaterial != null)
            {
                _meshMaterial.renderQueue = 2000 + state.LayerOrder;
            }
        }
    }
}
