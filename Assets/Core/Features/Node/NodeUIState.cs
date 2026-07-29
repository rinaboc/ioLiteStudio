using Core.Domain;

namespace Core.Features
{
    public abstract record NodeUIState(
        int ID,
        string Name,
        int? ParentID,
        bool Visibility,
        Transform2D DefaultTransform,
        Transform2D LocalTransform
    );
}
