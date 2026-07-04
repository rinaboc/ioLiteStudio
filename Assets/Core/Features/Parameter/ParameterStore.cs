using Core.Domain;

namespace Core.Features
{
    public class ParameterStore : Store<ParameterUIState>
    {
        public ParameterStore(ParameterUIState state, IDispatcher dispatcher) : base(state, dispatcher) { }

        protected override ContextMutationType[] ObservedMutations => new ContextMutationType[]
        {
            ContextMutationType.Parameter
        };

        protected override ParameterUIState Build(IAppContext context)
        {
            return new ParameterUIState(

            );
        }
    }
}
