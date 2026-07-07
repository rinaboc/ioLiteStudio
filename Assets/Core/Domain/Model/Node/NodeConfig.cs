namespace Core.Domain
{
    public record NodeConfig(string Name, bool Visibility = true, Transform2D DefaultTransform = null);
}
