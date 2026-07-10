using Core.Domain;

namespace Core.Features
{
    public class TopBarStore : Store<TopBarUIState>
    {
        public TopBarStore(TopBarUIState state, IDispatcher dispatcher) : base(state, dispatcher)
        {
        }

        protected override ContextMutationType[] ObservedMutations => new ContextMutationType[] { ContextMutationType.DEFAULT };

        protected override TopBarUIState Build(IAppContext context)
        {
            return new TopBarUIState(
                true,
                context.CurrentSession.CurrentWorkspace,
                context.CurrentSession.UndoCount > 0,
                context.CurrentSession.RedoCount > 0
            );
        }
    }
}
