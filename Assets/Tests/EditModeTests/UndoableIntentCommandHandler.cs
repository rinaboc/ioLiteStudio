using System;
using System.Threading.Tasks;
using Core.Domain;
using UnityEngine;

public record UndoableIntent : IIntent;

public class UndoableIntentCommandHandler : ICommandHandler<UndoableIntent>
{
    public CommandResult Execute(UndoableIntent intent, IAppContext context)
    {
        return new CommandResult(
            Mutation: () => { },
            Revert: () => { },
            new ContextMutationResult(new ContextMutationType[] { })
        );
    }
}

public record AsyncUndoableIntent : IIntent;

public class AsyncUndoableIntentCommandHandler : IAsyncCommandHandler<AsyncUndoableIntent>
{
    public async Awaitable<CommandResult> Execute(AsyncUndoableIntent intent, IAppContext context)
    {
        var dummyResult = new CommandResult(
            Mutation: () => { },
            Revert: () => { },
            new ContextMutationResult(new ContextMutationType[] { })
        );

        // heavy computations simulation
        await Task.Delay(30);

        return dummyResult;
    }
}