namespace Core.Features
{
    public abstract record InspectorUIState(
        bool IsInspectorOpen = true
    );
}