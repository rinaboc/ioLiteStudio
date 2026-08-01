using Core.Domain;

namespace Core.Features
{
    public record ReparentNodeBySiblingIntent(int NodeID, int? SiblingID) : IIntent;
}