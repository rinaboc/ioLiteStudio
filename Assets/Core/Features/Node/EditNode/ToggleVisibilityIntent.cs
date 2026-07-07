using Core.Domain;

namespace Core.Features
{
    public record ToggleVisibilityIntent(int ID) : IIntent;
}
