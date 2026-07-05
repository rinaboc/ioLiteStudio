using System;
using Core.Domain;

namespace Core.Features
{
    public record ParameterConfig(
        string Name, float Max, float Min, BlendMode BlendMode
    );
}
