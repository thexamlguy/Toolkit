namespace Toolkit.Foundation
{
    public record Write<TConfiguration>(string Section, Action<TConfiguration> UpdateDelegate) where TConfiguration : class;
}
