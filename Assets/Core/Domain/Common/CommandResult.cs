using System;
using Core.Domain;

namespace Core.Domain
{
    public record CommandResult(Action Mutation, Action Revert, ContextMutationResult MutationResult);
}