using System;
using Core.Domain;

namespace Core.Features
{
    public class CreateParameterCommandHandler : ICommandHandler<CreateParameterIntent>
    {
        public CommandResult Execute(CreateParameterIntent intent, IAppContext context)
        {
            var parameter = new Parameter()
            {
                Id = Guid.NewGuid(),
                Name = intent.Config.Name,
                MinValueFloat = intent.Config.Min,
                MaxValueFloat = intent.Config.Max,
                BlendMode = intent.Config.BlendMode,
                CurrentValueFloat = (intent.Config.Min + intent.Config.Max) * 0.5f
            };

            void mutation()
            {
                context.Parameters.Register(parameter.Id, parameter);
            }

            void revert()
            {
                context.Parameters.Remove(parameter.Id, out _);
            }

            return new CommandResult(
                Mutation: mutation,
                Revert: revert,
                MutationResult: new ContextMutationResult(
                    new[] { ContextMutationType.Parameter }
                )
            );
        }
    }
}
