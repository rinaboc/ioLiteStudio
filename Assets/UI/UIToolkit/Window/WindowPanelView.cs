using Core.Domain;
using Core.Features;
using UnityEngine.UIElements;

[UxmlElement]
public abstract partial class WindowPanelView : VisualElementView<WindowUIState>
{
    private Label _windowTitle;
    protected abstract WindowType PanelWindowType { get; }
    protected abstract string PanelTitle { get; }
    protected WindowPanelView()
    {
    }

    protected WindowPanelView(IViewModel<WindowUIState> viewModel, Label windowTitle) : base(viewModel)
    {
        _windowTitle = windowTitle;
    }

    protected virtual void ConfigurePanel(WindowUIState state)
    {
        _windowTitle.text = PanelTitle;
    }

    public override sealed void Render(WindowUIState state)
    {
        if (state.WindowType == PanelWindowType)
        {
            RemoveFromClassList("hide");
            ConfigurePanel(state);
        }
        else
        {
            AddToClassList("hide");
        }
    }
}