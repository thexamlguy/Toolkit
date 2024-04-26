using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class ServiceScopeFactory<TService>(IServiceScopeFactory serviceScopeFactory,
    ICache<TService, IServiceScope> cache) :
    IServiceScopeFactory<TService>
    where TService : notnull
{
    public TService? Create(params object?[] parameters)
    {
        if (serviceScopeFactory.CreateScope() is IServiceScope serviceScope)
        {
            if (serviceScope.ServiceProvider.GetService<IServiceFactory>() is IServiceFactory factory)
            {
                if (factory.Create<TService>(parameters) is TService service)
                {
                    cache.Add(service, serviceScope);
                    return service;
                }
            }
        }

        return default;
    }
}