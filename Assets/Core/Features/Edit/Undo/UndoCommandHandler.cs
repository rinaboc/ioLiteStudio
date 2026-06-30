using Core.Domain;
using Core.Services;

namespace Core.Features
{
    public class UndoCommandHandler : ICommandHandler<UndoIntent>
    {
        public CommandResult Execute(UndoIntent intent, IAppContext context)
        {
            var undoService = ServiceLocator.Get<IUndoService>();
            var undoResult = undoService.UndoCount > 0 ?
                undoService.LastStep.CommandResult.MutationResult : null;

            return new CommandResult(
                Mutation: () =>
                {
                    if (!undoService.Undo(out var revert)) return;

                    revert?.Invoke();
                    context.CurrentSession.UndoCount = undoService.UndoCount;
                    context.CurrentSession.RedoCount = undoService.RedoCount;
                },
                Revert: null,
                MutationResult: undoResult
            );
        }
    }
}
