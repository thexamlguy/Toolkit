using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public interface IComponentBuilder
{
    string ConfigurationFile { get; set; }

    string ContentRoot { get; set; }

    IComponentBuilder AddConfiguration<TConfiguration>(Action<TConfiguration> configurationDelegate)
        where TConfiguration : class, new();

    IComponentBuilder AddConfiguration<TConfiguration>(string section,
        TConfiguration? configuration = null)
        where TConfiguration : class, new();

    IComponentBuilder AddConfiguration<TConfiguration>(string section)
        where TConfiguration : class, new();

    IComponentBuilder AddServices(Action<IServiceCollection> configureDelegate);

    IComponentHost Build();
}