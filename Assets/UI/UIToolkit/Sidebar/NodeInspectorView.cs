using Core.Domain;
using Core.Features;
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

    }

    private void OnAddNodeClicked()
    {
        _viewModel.Dispatch(new OpenWindowIntent(WindowType.CreateNode));
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
            _nodeList.Add(new NodeInspectorElementView(node));
        }
    }
}
