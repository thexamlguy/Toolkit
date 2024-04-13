using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public interface IComponentBuilder
{
    IComponentBuilder AddConfiguration<TConfiguration>(Action<TConfiguration> configurationDelegate)
        where TConfiguration : ComponentConfiguration, new();

    IComponentHost Build();

    IComponentBuilder AddServices(Action<IServiceCollection> configureDelegate);
}