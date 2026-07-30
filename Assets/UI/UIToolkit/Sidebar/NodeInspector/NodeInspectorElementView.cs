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
    private VisualElement _folderHandle;

    private NodeInspectorElementCallbacks _inputCallbacks;

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

        _folderHandle = new()
        {
            pickingMode = PickingMode.Ignore
        };
        _folderHandle.AddToClassList("tree-span");
        _folderHandle.style.visibility = Visibility.Hidden;

        VisualElement span = new()
        {
            pickingMode = PickingMode.Ignore
        };
        span.style.flexGrow = 1;

        Add(_folderHandle);
        Add(_nodeName);
        Add(_editNodeName);
        Add(span);
        Add(_visibilityToggle);
    }

    private void OnVisibilityToggled(ChangeEvent<bool> evt)
    {
        _inputCallbacks.OnVisibilityToggled?.Invoke();
    }

    private void OnNameChanged(ChangeEvent<string> evt)
    {
        _editNodeName.AddToClassList("hide");
        _nodeName.RemoveFromClassList("hide");

        _inputCallbacks.OnSubmit?.Invoke(evt.newValue);
    }

    public NodeInspectorElementView(NodeInspectorElement node) : this()
    {
        _nodeName.text = node.Name;
        _editNodeName.SetValueWithoutNotify(node.Name);
        _visibilityToggle.SetValueWithoutNotify(node.IsVisible);

        if (node.Type == Core.Domain.NodeType.Deformer)
        {
            AddToClassList("deformer-node");
        }

        if (node.HasChildren)
        {
            _folderHandle.style.visibility = Visibility.Visible;
            _folderHandle.pickingMode = PickingMode.Position;
            _folderHandle.RegisterCallback<MouseDownEvent>(OnFolderHandleClicked, TrickleDown.NoTrickleDown);

            if (node.IsOpened)
                _folderHandle.AddToClassList("close-folder-icon");
        }

        if (node.IsSelected)
            AddToClassList("selected-element");

        for (int i = 0; i < node.Depth; i++)
        {
            var depthSpan = new VisualElement()
            {
                pickingMode = PickingMode.Ignore
            };

            depthSpan.style.visibility = Visibility.Hidden;
            depthSpan.AddToClassList("tree-span");
            Add(depthSpan);
            depthSpan.SendToBack();
        }
    }

    private void OnFolderHandleClicked(MouseDownEvent evt)
    {
        _inputCallbacks.OnFolderHandleToggled?.Invoke();
    }

    public NodeInspectorElementView(NodeInspectorElement node, NodeInspectorElementCallbacks callbacks) : this(node)
    {
        _inputCallbacks = callbacks;

        RegisterCallback<MouseDownEvent>(OnContainerClicked, TrickleDown.NoTrickleDown);
        RegisterCallback<DoubleClickEvent>(OnContinerDoubleClicked, TrickleDown.NoTrickleDown);
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
        UnregisterCallback<MouseDownEvent>(OnContainerClicked);
        UnregisterCallback<MouseDownEvent>(OnFolderHandleClicked);
        UnregisterCallback<DoubleClickEvent>(OnContinerDoubleClicked);
    }
}