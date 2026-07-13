using System;
using Core.Domain;
using Core.Features;
using UnityEngine.UIElements;

[UxmlElement]
public partial class FilePanelView : WindowPanelView
{
    private Button _newProjectButton;
    private Button _saveProjectButton;
    private Button _loadProjectButton;

    protected override WindowType PanelWindowType => WindowType.File;

    protected override string PanelTitle => "File";

    public FilePanelView() { }
    public FilePanelView(IViewModel<WindowUIState> viewModel, Label windowTitle) : base(viewModel, windowTitle)
    {
        AddToClassList("panel");

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
}
