using Core.Domain;

namespace Core.Features
{
    public record OpenWindowIntent(WindowType WindowType) : IIntent;
}
