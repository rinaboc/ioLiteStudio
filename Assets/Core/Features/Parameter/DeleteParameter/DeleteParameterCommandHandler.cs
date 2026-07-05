using Core.Domain;

namespace Core.Features
{
    public class DeleteParameterCommandHandler : ICommandHandler<DeleteParameterIntent>
    {
        public CommandResult Execute(DeleteParameterIntent intent, IAppContext context)
        {
            if (!context.Parameters.TryGet(intent.ID, out var parameter))
            {
                ServiceLocator.Get<ILogService>()?.LogError("[DELETE_PARAMETER] No parameter with this ID found.");
                return new CommandResult(null, null, null);
            }

            void mutation()
            {
                context.Parameters.Remove(parameter.Id, out _);
            }

            void revert()
            {
                context.Parameters.Register(parameter.Id, parameter);
            }

            return new CommandResult(
                mutation,
                revert,
                new ContextMutationResult(new ContextMutationType[]
                {
                    ContextMutationType.Parameter
                })
            );

        }
    }
}
