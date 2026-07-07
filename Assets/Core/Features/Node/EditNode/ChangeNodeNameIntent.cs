using Core.Domain;

namespace Core.Features
{
    public record ChangeNodeNameIntent(int ID, string Name) : IIntent;
}
