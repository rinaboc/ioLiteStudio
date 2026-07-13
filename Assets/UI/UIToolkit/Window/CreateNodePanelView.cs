using System.Collections.Generic;
using System.Linq;
using Core.Domain;
using Core.Features;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class CreateNodePanelView : WindowPanelView
{
    private Button _createButton;
    [SerializeField] private string _name;
    [SerializeField] private string _texPath;
    [SerializeField] private NodeType? _nodeType;
    private Button _textureSelectorButton;
    [SerializeField] private string _textureSelectionText;
    private Label _textureSelectionLabel;
    private string[] _texturePaths;
    private RadioButtonGroup _nodeTypeSelection;

    public CreateNodePanelView() { }

    public CreateNodePanelView(IViewModel<WindowUIState> viewModel, Label windowTitle) : base(viewModel, windowTitle)
    {
        var nameTextField = new TextField("Name");
        nameTextField.SetBinding("value", new DataBinding
        {
            dataSource = this,
            dataSourcePath = new PropertyPath(nameof(_name)),
            bindingMode = BindingMode.TwoWay
        });

        Add(nameTextField);

        List<string> nodeTypes = new(new string[] { "Mesh", "Node" });
        _nodeTypeSelection = new RadioButtonGroup("Node type", nodeTypes);
        _nodeTypeSelection.RegisterValueChangedCallback(OnNodeTypeSelected);

        Add(_nodeTypeSelection);

        _textureSelectorButton = new Button(OnTextureSelectorClicked)
        {
            text = "Load Texture"
        };
        _textureSelectorButton.AddToClassList("hide");

        Add(_textureSelectorButton);

        _textureSelectionLabel = new Label("");
        _textureSelectionLabel.SetBinding("text", new DataBinding
        {
            dataSource = this,
            dataSourcePath = new PropertyPath(nameof(_textureSelectionText)),
            bindingMode = BindingMode.TwoWay
        });

        Add(_textureSelectionLabel);

        _createButton = new Button(OnCreateClicked)
        {
            text = "Create"
        };

        Add(_createButton);
    }

    private async void OnTextureSelectorClicked()
    {
        _texturePaths = await ServiceLocator.Get<IImageService>().OpenImageFile();
        _textureSelectionText = $"Selected {_texturePaths.Count()} texture(s).";
    }

    private void OnNodeTypeSelected(ChangeEvent<int> evt)
    {
        if (evt.newValue == 0)
        {
            _nodeType = NodeType.Mesh;
            _textureSelectorButton.RemoveFromClassList("hide");
            _textureSelectionLabel.RemoveFromClassList("hide");
        }
        else
        {
            _nodeType = NodeType.Deformer;
            _textureSelectorButton.AddToClassList("hide");
            _textureSelectionLabel.AddToClassList("hide");
        }
    }

    private void OnCreateClicked()
    {
        if (_name == "" || _nodeType == null)
            return;

        if (_nodeType == NodeType.Deformer)
        {
            _viewModel.Dispatch(new CreateDeformerNodeIntent(new DeformerNodeConfig(new NodeConfig(_name, DefaultTransform: Transform2D.Default))));
            _viewModel.Dispatch(new CloseWindowIntent());
            return;
        }

        if (_texturePaths == null)
            return;

        foreach (var path in _texturePaths)
        {
            _viewModel.Dispatch(new CreateMeshNodeIntent(new MeshNodeConfig(
                new NodeConfig(_name, DefaultTransform: Transform2D.Default),
                path,
                new MeshInfo()
            )));
            _viewModel.Dispatch(new CloseWindowIntent());
        }
    }

    protected override void ConfigurePanel(WindowUIState state)
    {
        base.ConfigurePanel(state);
        _name = "";
        _textureSelectionText = "";
        _nodeTypeSelection.value = -1;
    }

    protected override WindowType PanelWindowType => WindowType.CreateNode;

    protected override string PanelTitle => "Create New Node";

    public override void Dispose()
    {
        base.Dispose();
        _createButton.clicked -= OnCreateClicked;
        _textureSelectorButton.clicked -= OnTextureSelectorClicked;
    }
}