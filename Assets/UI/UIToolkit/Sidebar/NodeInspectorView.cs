using Core.Domain;
using Core.Features;
using UI.UIToolkit;
using UnityEngine.UIElements;

public class NodeInspectorView : View<NodeInspectorUIState>
{
    private Button _addNodeButton;
    private Button _deleteNodeButton;
    private VisualElement _nodeList;

    public NodeInspectorView(IViewModel<NodeInspectorUIState> viewModel, VisualElement inspectorRoot) : base(viewModel)
    {
        _addNodeButton = inspectorRoot.Q<Button>("AddNodeBtn");
        _deleteNodeButton = inspectorRoot.Q<Button>("DeleteNodeBtn");

        _nodeList = inspectorRoot.Q<VisualElement>("NodeList");

        _addNodeButton.clicked += OnAddNodeClicked;
        _deleteNodeButton.clicked += OnDeleteNodeClicked;
    }

    private void OnDeleteNodeClicked()
    {
        var selectedNode = _viewModel.State.SelectedElement;
        if (selectedNode == null) return;

        _viewModel?.Dispatch(new DeleteNodeIntent((int)selectedNode));
    }

    private void OnAddNodeClicked()
    {
        _viewModel?.Dispatch(new OpenWindowIntent(WindowType.CreateNode));
    }

    public override void Dispose()
    {
        base.Dispose();
        _addNodeButton.clicked -= OnAddNodeClicked;
        _deleteNodeButton.clicked -= OnDeleteNodeClicked;
    }

    public override void Render(NodeInspectorUIState state)
    {
        _nodeList.Clear();

        foreach (var node in state.Elements)
        {
            _nodeList.Add(new NodeInspectorElementView(node, new InspectorElementCallbacks(
                OnClick: () => _viewModel?.Dispatch(new SelectNodeIntent(node.ID)),
                OnSubmit: (string name) => _viewModel?.Dispatch(new ChangeNodeNameIntent(node.ID, name)),
                OnVisibilityToggled: () => _viewModel?.Dispatch(new ToggleVisibilityIntent(node.ID))
            )));
        }
    }
}
