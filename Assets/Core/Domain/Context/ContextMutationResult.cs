namespace Core.Domain
{
    public enum ContextMutationType
    {

    }

    public record ContextMutationResult(ContextMutationType[] ContextMutations);
}