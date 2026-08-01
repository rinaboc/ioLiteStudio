using System;

namespace Core.Domain
{
    public record CommandResult(Action Mutation, Action Revert, ContextMutationResult MutationResult)
    {
        public static CommandResult None => new CommandResult(null, null, null);
        public static CommandResult MutationOnly(Action Mutation, ContextMutationResult MutationResult) => new CommandResult(Mutation, null, MutationResult);
    };
}