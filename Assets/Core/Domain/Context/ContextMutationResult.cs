namespace Core.Domain
{
    public enum ContextMutationType
    {
        Parameter
    }

    public record ContextMutationResult(ContextMutationType[] ContextMutations);
}