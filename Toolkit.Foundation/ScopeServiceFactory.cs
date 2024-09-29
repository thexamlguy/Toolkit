using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class ScopeServiceFactory<TService>(IServiceScopeFactory serviceScopeFactory,
    ICache<TService, IServiceScope> cache) :
    IScopeServiceFactory<TService>
    where TService : notnull
{
    public (IServiceScope, TService)? Create(params object?[] parameters)
    {
        if (serviceScopeFactory.CreateScope() is IServiceScope serviceScope)
        {
            if (serviceScope.ServiceProvider.GetService<IServiceFactory>() is IServiceFactory factory)
            {
                if (factory.Create<TService>(parameters) is TService service)
                {
                    cache.Add(service, serviceScope);
                    return (serviceScope, service);
                }
            }
        }

        return default;
    }
}