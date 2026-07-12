using Core.Domain;
using UnityEngine.UIElements;

[UxmlElement]
public abstract partial class VisualElementView<TState> : VisualElement, IView<TState>
{
    protected IViewModel<TState> _viewModel;

    protected VisualElementView() { }

    public VisualElementView(IViewModel<TState> viewModel) : base()
    {
        _viewModel = viewModel;
    }

    public virtual void Dispose()
    {
        _viewModel = null;
    }

    public abstract void Render(TState state);
}
