using System;
using Core.Features;
using Input;
using UI.UIToolkit;
using UnityEngine.UIElements;

[UxmlElement]
public partial class NodeInspectorElementView : VisualElement
{
    private Label _nodeName;
    private TextField _editNodeName;
    private Toggle _visibilityToggle;

    private InspectorElementCallbacks _inputCallbacks;

    public NodeInspectorElementView()
    {
        AddToClassList("node-container");

        _nodeName = new Label("Node Name")
        {
            pickingMode = PickingMode.Ignore
        };

        _editNodeName = new TextField()
        {
            pickingMode = PickingMode.Ignore,
            isDelayed = true,
        };
        _editNodeName.AddToClassList("hide");

        _editNodeName.RegisterValueChangedCallback(OnNameChanged);

        _visibilityToggle = new Toggle()
        {

        };
        _visibilityToggle.AddToClassList("iolite-toggle");
        _visibilityToggle.RegisterValueChangedCallback(OnVisibilityToggled);

        Add(_nodeName);
        Add(_editNodeName);
        Add(_visibilityToggle);
    }

    private void OnVisibilityToggled(ChangeEvent<bool> evt)
    {
        _inputCallbacks.OnVisibilityToggled?.Invoke();
    }

    private void OnNameChanged(ChangeEvent<string> evt)
    {
        _inputCallbacks.OnSubmit?.Invoke(evt.newValue);
    }

    public NodeInspectorElementView(NodeInspectorElement node) : this()
    {
        _nodeName.text = node.Name;
        _editNodeName.SetValueWithoutNotify(node.Name);
        _visibilityToggle.SetValueWithoutNotify(node.IsVisible);

        if (node.IsSelected)
            AddToClassList("selected-element");
    }

    public NodeInspectorElementView(NodeInspectorElement node, InspectorElementCallbacks callbacks) : this(node)
    {
        _inputCallbacks = callbacks;

        RegisterCallback<MouseDownEvent>(OnContainerClicked, CallbackOptions.Removable);
        RegisterCallback<DoubleClickEvent>(OnContinerDoubleClicked, CallbackOptions.Removable);
    }

    private void OnContinerDoubleClicked(DoubleClickEvent evt)
    {
        if (_editNodeName.ClassListContains("hide"))
        {
            _editNodeName.RemoveFromClassList("hide");
            _editNodeName.Focus();
            _nodeName.AddToClassList("hide");
        }
    }

    private void OnContainerClicked(MouseDownEvent evt)
    {
        _inputCallbacks.OnClick?.Invoke();
    }

    ~NodeInspectorElementView()
    {
        UnregisterAllRemovableCallbacks();
    }
}