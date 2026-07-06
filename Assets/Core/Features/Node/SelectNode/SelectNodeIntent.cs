using Core.Domain;

namespace Core.Features
{
    public record SelectNodeIntent(int? ID) : IIntent;
}
