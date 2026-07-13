using Core.Features;
using UnityEngine.UIElements;

[UxmlElement]
public partial class NodeInspectorElementView : VisualElement
{
    private Label _nodeName;
    public NodeInspectorElementView()
    {
        _nodeName = new Label("Node Name");
        Add(_nodeName);
    }

    public NodeInspectorElementView(NodeInspectorElement node) : this()
    {
        _nodeName.text = node.Name;
    }
}