using Core.Domain;

namespace Core.Features
{
    public record ReparentNodeIntent(int NodeID, int? ParentID) : IIntent;
}
