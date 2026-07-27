using Core.Domain;

namespace Core.Features
{
    public record WidgetToggleIntent(Widget Widget, bool IsOpen) : IIntent;
}