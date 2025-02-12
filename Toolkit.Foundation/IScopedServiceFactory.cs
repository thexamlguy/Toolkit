using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public interface IScopedServiceFactory<TService>
{
    (IServiceProvider, TService) Create(params object?[] parameters);
}