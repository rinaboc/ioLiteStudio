using Core.Domain;

namespace Core.Features
{
    public record CreateDeformerNodeIntent(DeformerNodeConfig DeformerNodeConfig) : IIntent;
}
