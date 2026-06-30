using System;

namespace Core.Domain
{
    public record CommandResult(Action Mutation, Action Revert, ContextMutationResult MutationResult);
}