using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public interface IComponentFactory
{
    IComponentHost? Create<TComponent, TConfiguration>(string name,
        TConfiguration? configuration = null,
        Action<IComponentBuilder>? builderDelegate = null,
        Action<IServiceCollection>? servicesDelegate = null)
        where TComponent : IComponent
        where TConfiguration : class, new();
}