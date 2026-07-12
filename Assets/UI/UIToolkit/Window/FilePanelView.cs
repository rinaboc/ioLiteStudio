using System;
using Core.Domain;
using Core.Features;
using UnityEngine.UIElements;

[UxmlElement]
public partial class FilePanelView : VisualElementView<WindowUIState>
{
    private Button _newProjectButton;
    private Button _saveProjectButton;
    private Button _loadProjectButton;
    private Label _windowTitle;

    public FilePanelView() { }
    public FilePanelView(IViewModel<WindowUIState> viewModel, Label windowTitle) : base(viewModel)
    {
        AddToClassList("panel");
        _windowTitle = windowTitle;

        _newProjectButton = new Button(OnNewProjectClicked)
        {
            text = "New Project"
        };

        _saveProjectButton = new Button(OnSaveProjectClicked)
        {
            text = "Save Project"
        };

        _loadProjectButton = new Button(OnLoadProjectClicked)
        {
            text = "Load Project"
        };

        Add(_newProjectButton);
        Add(_saveProjectButton);
        Add(_loadProjectButton);
    }

    public override void Dispose()
    {
        base.Dispose();
        _newProjectButton.clicked -= OnNewProjectClicked;
        _saveProjectButton.clicked -= OnSaveProjectClicked;
        _loadProjectButton.clicked -= OnLoadProjectClicked;
    }

    private void OnLoadProjectClicked()
    {
        throw new NotImplementedException();
    }

    private void OnSaveProjectClicked()
    {
        throw new NotImplementedException();
    }

    private void OnNewProjectClicked()
    {
        throw new NotImplementedException();
    }

    public override void Render(WindowUIState state)
    {
        if (state.WindowType == WindowType.File)
        {
            RemoveFromClassList("hide");
            _windowTitle.text = "File";
        }
        else
        {
            AddToClassList("hide");
        }
    }
}
