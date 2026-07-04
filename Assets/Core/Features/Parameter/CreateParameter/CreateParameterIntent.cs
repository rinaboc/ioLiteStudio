using Core.Domain;

namespace Core.Features
{
    public record CreateParameterIntent(ParameterConfig Config) : IIntent;
}
