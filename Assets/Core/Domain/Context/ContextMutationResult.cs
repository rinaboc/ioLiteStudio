namespace Core.Domain
{
    public enum ContextMutationType
    {
        Parameter, Node
    }

    public record ContextMutationResult(params ContextMutationType[] ContextMutations);
}