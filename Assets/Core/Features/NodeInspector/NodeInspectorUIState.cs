namespace Core.Features
{
    public record NodeInspectorUIState(
        NodeInspectorElement[] Elements,
        int? SelectedElement,
        bool IsInspectorOpen = true
    ) : InspectorUIState(IsInspectorOpen);
}