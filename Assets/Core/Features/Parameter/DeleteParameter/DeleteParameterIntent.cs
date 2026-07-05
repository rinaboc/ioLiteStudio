using System;
using Core.Domain;

namespace Core.Features
{
    public record DeleteParameterIntent(Guid ID) : IIntent;
}
