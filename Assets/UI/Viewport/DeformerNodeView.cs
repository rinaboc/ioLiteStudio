using Core.Domain;
using Core.Features;
using UnityEngine;

namespace UI.Viewport
{
    public class DeformerNodeView : View<DeformerNodeUIState>
    {
        private GameObject _deformerObject;
        public DeformerNodeView(IViewModel<DeformerNodeUIState> viewModel, GameObject deformerObject) : base(viewModel)
        {
            _deformerObject = deformerObject;
        }

        public override void Render(DeformerNodeUIState state)
        {
            var transform = state.DefaultTransform + state.LocalTransform;
            _deformerObject.transform.position = new Vector3(transform.Position.x, transform.Position.y, 0f);
            _deformerObject.transform.localScale = new Vector3(transform.Scale.y, transform.Scale.y, 0f);
            _deformerObject.transform.rotation = transform.Rotation;
            _deformerObject.SetActive(state.Visibility);
        }
    }
}
