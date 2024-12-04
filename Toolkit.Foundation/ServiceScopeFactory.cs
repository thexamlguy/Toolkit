using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class ServiceScopeFactory<TServiceScope>(IServiceScopeFactory serviceScopeFactory,
    ICache<TServiceScope, IServiceScope> cache) :
    IServiceScopeFactory<TServiceScope>
    where TServiceScope : notnull
{
    public (IServiceScope, TServiceScope) Create(params object?[] parameters)
    {
        if (serviceScopeFactory.CreateScope() is IServiceScope serviceScope)
        {
            if (serviceScope.ServiceProvider.GetService<IServiceFactory>() is IServiceFactory factory)
            {
                if (factory.Create<TServiceScope>(parameters) is TServiceScope service)
                {
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