namespace Core.Domain
{
    public record HistoryStep(IIntent Intent, CommandResult CommandResult);
}
