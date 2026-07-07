using Core.Domain;

namespace Core.Features
{
    public record DeleteNodeIntent(int ID) : IIntent;
}
