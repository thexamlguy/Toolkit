using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class ScopedServiceFactory<TScopedService>(IServiceScopeFactory serviceScopeFactory,
    ICache<TScopedService, IServiceScope> cache) :
    IScopedServiceFactory<TScopedService>
    where TScopedService : notnull
{
    public (IServiceScope, TScopedService) Create(params object?[] parameters)
    {
        if (serviceScopeFactory.CreateScope() is IServiceScope serviceScope)
        {
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            if (serviceProvider.GetRequiredService<IServiceFactory>() is IServiceFactory factory)
            {
                if (factory.Create<TScopedService>(parameters) is TScopedService service)
                {
                    serviceProvider.GetRequiredService<IScopedServiceDescriptor<TScopedService>>().Set(service);
                    cache.Add(service, serviceScope);

                    foreach (IInitializationScoped initializationScoped in serviceScope.ServiceProvider.GetServices<IInitializationScoped>())
                    {
                        initializationScoped.Initialize();
                    }

                    return (serviceScope, service);
                }
            }
        }

        return default;
    }
}