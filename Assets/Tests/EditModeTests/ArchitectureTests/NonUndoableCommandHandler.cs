using System.Threading.Tasks;
using Core.Domain;
using UnityEngine;

public record NonUndoableIntent : IIntent;

public class NonUndoableIntentCommandHandler : ICommandHandler<NonUndoableIntent>
{
    public CommandResult Execute(NonUndoableIntent intent, IAppContext context)
    {
        return new CommandResult(
            Mutation: () => { },
            Revert: null,
            new ContextMutationResult(new ContextMutationType[] { })
        );
    }
}

public record AsyncNonUndoableIntent : IIntent;

public class AsyncNonUndoableIntentCommandHandler : IAsyncCommandHandler<AsyncNonUndoableIntent>
{
    public async Awaitable<CommandResult> Execute(AsyncNonUndoableIntent intent, IAppContext context)
    {
        var dummyResult = new CommandResult(
            Mutation: () => { },
            Revert: null,
            new ContextMutationResult(new ContextMutationType[] { })
        );

        // heavy computations simulation
        await Task.Delay(30);

        return dummyResult;
    }
}
