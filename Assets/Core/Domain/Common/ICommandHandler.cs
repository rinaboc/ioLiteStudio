using Core.Domain;

namespace Core.Domain
{
    public interface ICommandHandler<TIntent> where TIntent : IIntent
    {
        CommandResult Execute(TIntent intent, IAppContext context);
    }
}