using System;
using Core.Domain;

namespace Core.Features
{
    public record EditParameterIntent(Guid ID, ParameterConfig Config) : IIntent;
}
