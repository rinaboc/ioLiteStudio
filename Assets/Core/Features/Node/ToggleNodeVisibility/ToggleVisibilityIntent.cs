using Core.Domain;

namespace Core.Features
{
    public record ToggleNodeVisibilityIntent(int ID) : IIntent;
}
