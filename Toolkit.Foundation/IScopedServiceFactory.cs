using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public interface IScopedServiceFactory<TService>
{
    (IServiceScope, TService) Create(params object?[] parameters);
}