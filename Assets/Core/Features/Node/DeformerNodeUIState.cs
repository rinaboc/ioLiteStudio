using Core.Domain;

namespace Core.Features
{
    public record DeformerNodeUIState(
        int ID,
        string Name,
        int? ParentID,
        bool Visibility,
        Transform2D DefaultTransform,
        Transform2D LocalTransform
    ) : NodeUIState(ID, Name, ParentID, Visibility, DefaultTransform, LocalTransform);

    public static class DeformerNodeExtension
    {
        public static DeformerNodeUIState ToDeformerNodeUIState(this DeformerNode n)
        {
            return new DeformerNodeUIState(
                n.Id,
                n.Name,
                n.ParentId,
                n.Visibility,
                n.DefaultTransform,
                n.LocalTransform
            );
        }
    }
}
