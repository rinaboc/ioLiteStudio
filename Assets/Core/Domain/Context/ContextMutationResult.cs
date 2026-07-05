namespace Core.Domain
{
    public enum ContextMutationType
    {
        Parameter
    }

    public record ContextMutationResult(params ContextMutationType[] ContextMutations);
}