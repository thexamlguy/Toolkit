namespace Toolkit.Foundation;

public class ScopedServiceProvider<TService>(ICache<TService, IServiceProvider> cache) :
    IScopedServiceProvider<TService>
    where TService : notnull
{
    public bool TryGet(TService service,
        out IServiceProvider? serviceProvider)
    {
        if (cache.TryGetValue(service, out IServiceProvider? value))
        {
            serviceProvider = value;
            return true;
        }

        serviceProvider = null;
        return false;
    }
}