using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public interface IComponentBuilder
{
    IComponentBuilder AddConfiguration<TConfiguration>(Action<TConfiguration> configurationDelegate)
        where TConfiguration : ComponentConfiguration, new();

    IComponentBuilder AddConfiguration<TConfiguration>(string section,
        TConfiguration? configuration = null)
        where TConfiguration : ComponentConfiguration, new();

    IComponentBuilder AddConfiguration<TConfiguration>(string section)
        where TConfiguration : ComponentConfiguration, new();

    IComponentBuilder AddServices(Action<IServiceCollection> configureDelegate);

    IComponentHost Build();

    void SetComponentConfiguration(Action<ComponentContentConfiguration> configurationDelegate);
}