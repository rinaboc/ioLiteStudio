using Core.Domain;
using Core.Services;

namespace Core.Features
{
    public class RedoCommandHandler : ICommandHandler<RedoIntent>
    {
        public CommandResult Execute(RedoIntent intent, IAppContext context)
        {
            var undoService = ServiceLocator.Get<IUndoService>();
            var redoResult = undoService.RedoCount > 0 ?
                undoService.NextStep.CommandResult.MutationResult : null;

            return new CommandResult(
                Mutation: () =>
                {
                    if (!undoService.Redo(out var mutation)) return;

                    mutation?.Invoke();
                    context.CurrentSession.UndoCount = undoService.UndoCount;
                    context.CurrentSession.RedoCount = undoService.RedoCount;
                },
                Revert: null,
                MutationResult: redoResult
            );
        }
    }
}
