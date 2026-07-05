using Core.Domain;

namespace Core.Features
{
    public class EditParameterCommandHandler : ICommandHandler<EditParameterIntent>
    {
        public CommandResult Execute(EditParameterIntent intent, IAppContext context)
        {
            if (!context.Parameters.TryGet(intent.ID, out var parameter))
                return new CommandResult(null, null, null);

            var prevConfig = parameter.Config;

            void mutation()
            {
                parameter.Config = intent.Config;
            }

            void revert()
            {
                parameter.Config = prevConfig;
            }

            return new CommandResult(
                mutation,
                revert,
                new ContextMutationResult(ContextMutationType.Parameter)
            );
        }
    }
}
