using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public interface IComponentFactory
{
    IComponentHost? Create<TComponent>(string name,
        ComponentConfiguration configuration, Action<IServiceCollection>? servicesDelegate = null)
        where TComponent : IComponent;
}