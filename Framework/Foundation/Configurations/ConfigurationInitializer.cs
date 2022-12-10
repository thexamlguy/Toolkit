using Mediator;

namespace Toolkit.Framework.Foundation;

public class ConfigurationInitializer<TConfiguration> : IInitializable where TConfiguration : class, new()
{
    private readonly TConfiguration configuration;
    private readonly IMediator mediator;

    public ConfigurationInitializer(TConfiguration configuration,
        IMediator mediator)
    {
        this.configuration = configuration;
        this.mediator = mediator;
    }

    public async Task InitializeAsync()
    {
        await mediator.Send(configuration);
        await Task.CompletedTask;
    }
}