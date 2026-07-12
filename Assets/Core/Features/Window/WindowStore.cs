using Core.Domain;

namespace Core.Features
{
    public class WindowStore : Store<WindowUIState>
    {
        public WindowStore(WindowUIState state, IDispatcher dispatcher) : base(state, dispatcher)
        {
        }

        protected override ContextMutationType[] ObservedMutations => new ContextMutationType[] { ContextMutationType.Session };

        protected override WindowUIState Build(IAppContext context)
        {
            return new WindowUIState(
                context.CurrentSession.IsWindowOpen,
                context.CurrentSession.WindowType
            );
        }
    }
}
