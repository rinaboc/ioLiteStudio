using Core.Domain;
using Core.Features;
using UnityEngine.UIElements;

public class WindowView : View<WindowUIState>
{
    private VisualElement _baseElement;

    public WindowView(IViewModel<WindowUIState> viewModel, VisualElement windowBase) : base(viewModel)
    {
        _baseElement = windowBase;
        _baseElement.RegisterCallback<MouseDownEvent>(OnBaseElementClick);

        var windowContainer = windowBase.Q<VisualElement>("WindowContainer");
        var windowTitle = windowBase.Q<Label>("WindowTitle");

        var filePanel = new FilePanelView(viewModel, windowTitle);
        windowContainer.Add(filePanel);
        _viewModel.Bind(filePanel);

        var createNodePanel = new CreateNodePanelView(viewModel, windowTitle);
        windowContainer.Add(createNodePanel);
        _viewModel.Bind(createNodePanel);
    }

    public override void Dispose()
    {
        base.Dispose();
        _baseElement.UnregisterCallback<MouseDownEvent>(OnBaseElementClick);
    }

    private void OnBaseElementClick(MouseDownEvent evt)
    {
        _viewModel.Dispatch(new CloseWindowIntent());
    }

    public override void Render(WindowUIState state)
    {
        if (state.IsOpen)
            _baseElement.RemoveFromClassList("hide");
        else
            _baseElement.AddToClassList("hide");
    }
}
