using System;
using Core.Domain;
using UnityEngine;

namespace Core.Features
{
    public record SelectParameterIntent(Guid ID) : IIntent;
}
