namespace Core.Domain
{
    public enum ContextMutationType
    {
        Parameter, Node, Session, DEFAULT
    }

    public record ContextMutationResult(params ContextMutationType[] ContextMutations);
}