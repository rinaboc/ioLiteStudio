using Core.Features;
using UnityEngine.UIElements;

[UxmlElement]
public partial class NodeInspectorElementView : VisualElement
{
    public NodeInspectorElementView()
    {
        Add(new Label("Node Name"));
    }

    public NodeInspectorElementView(NodeInspectorElement node) : this()
    {

    }
}