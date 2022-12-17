namespace Toolkit.Framework.Foundation;

public record ConfigurationChanged<TConfiguration>(TConfiguration Configuration) : INotification where TConfiguration : class;