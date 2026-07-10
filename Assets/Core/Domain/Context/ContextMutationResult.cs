namespace Core.Domain
{
    public enum ContextMutationType
    {
        Parameter, Node, DEFAULT
    }

    public record ContextMutationResult(params ContextMutationType[] ContextMutations);
}