using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public interface IComponentFactory
{
    IComponentHost? Create<TComponent, TConfiguration>(string key,
        Action<IConfigurationBuilder<TConfiguration>>? configurationDelegate = null,
        Action<IComponentBuilder>? componentDelegate = null,
        Action<IServiceCollection>? servicesDelegate = null)
        where TComponent : IComponent
        where TConfiguration : class, new();
}