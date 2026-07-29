using Core.Domain;
using Core.Features;
using UI.UIToolkit;
using UnityEngine.UIElements;

public class NodeInspectorView : InspectorView<NodeInspectorUIState>
{
    protected override string InspectorLabelText => "Nodes";
    protected override Widget InspectorWidget => Widget.Node;

    public NodeInspectorView(IViewModel<NodeInspectorUIState> viewModel, VisualElement inspectorRoot) : base(viewModel, inspectorRoot)
    {
    }

    protected override void OnDeleteClicked()
    {
        var selectedNode = _viewModel.State.SelectedElement;
        if (selectedNode == null) return;

        _viewModel?.Dispatch(new DeleteNodeIntent((int)selectedNode));
    }

    protected override void OnAddClicked()
    {
        _viewModel?.Dispatch(new OpenWindowIntent(WindowType.CreateNode));
    }

    public override void Render(NodeInspectorUIState state)
    {
        base.Render(state);

        _listElement.Clear();

        foreach (var node in state.Elements)
        {
            _listElement.Add(new NodeInspectorElementView(node, new NodeInspectorElementCallbacks(
                OnClick: () => _viewModel?.Dispatch(new SelectNodeIntent(node.ID)),
                OnSubmit: (string name) => _viewModel?.Dispatch(new ChangeNodeNameIntent(node.ID, name)),
                OnVisibilityToggled: () => _viewModel?.Dispatch(new ToggleVisibilityIntent(node.ID)),
                OnFolderHandleToggled: () => _viewModel?.Dispatch(new ToggleDeformerFolderIntent(node.ID))
            )));
        }
    }
}
