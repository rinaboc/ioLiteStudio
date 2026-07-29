namespace Core.Features
{
    public record NodeInspectorElement(
        int ID,
        string Name,
        int LayerOrder,
        int? ParentID,
        bool IsSelected,
        bool IsVisible,
        int Depth,
        bool HasChildren
    );
}