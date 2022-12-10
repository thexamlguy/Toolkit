namespace Toolkit.Framework.Foundation;

public record ConfigurationChanged<TConfiguration>(TConfiguration Configuration) where TConfiguration : class;