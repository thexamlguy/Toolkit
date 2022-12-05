namespace Toolkit.Foundation
{
    public record ConfigurationChanged<TConfiguration>(TConfiguration Configuration) where TConfiguration : class;
}
