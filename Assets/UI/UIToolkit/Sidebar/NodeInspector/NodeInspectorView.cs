using Core.Domain;
using Core.Features;
using UI.UIToolkit;
using UnityEngine.UIElements;

public class NodeInspectorView : InspectorView<NodeInspectorUIState>
{
    protected override string InspectorLabelText => "Nodes";
    protected override Widget InspectorWidget => Widget.Node;

    private int? _grabbedNodeID = null;

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
                OnSubmit: name => _viewModel?.Dispatch(new ChangeNodeNameIntent(node.ID, name)),
                OnVisibilityToggled: () => _viewModel?.Dispatch(new ToggleNodeVisibilityIntent(node.ID)),
                OnFolderHandleToggled: () => _viewModel?.Dispatch(new ToggleDeformerFolderIntent(node.ID)),
                OnGrab: () => OnGrabElement(node.ID),
                OnDrop: dropPosition => OnDropElement(node.ID, dropPosition)
            )));
        }
    }

    private void OnGrabElement(int grabbedNodeID)
    {
        _grabbedNodeID = grabbedNodeID;
    }

    private void OnDropElement(int dropNodeID, DropPosition dropPosition)
    {
        if (_grabbedNodeID == null) return;

        switch (dropPosition)
        {
            case DropPosition.Above: _viewModel?.Dispatch(new ReparentNodeBySiblingIntent((int)_grabbedNodeID, dropNodeID)); break;
            case DropPosition.Below: _viewModel?.Dispatch(new ReparentNodeBySiblingIntent((int)_grabbedNodeID, dropNodeID)); break;
            case DropPosition.Inside: _viewModel?.Dispatch(new ReparentNodeIntent((int)_grabbedNodeID, dropNodeID)); break;
        }
    }
}
