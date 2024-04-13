using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class ServiceScopeProvider<TService>(ICache<TService, IServiceScope> cache) :
    IServiceScopeProvider<TService>
    where TService : notnull
{
    public bool TryGet(TService service, 
        out IServiceScope? serviceScope)
    {
        if (cache.TryGetValue(service, out IServiceScope? value))
        {
            serviceScope = value;
            return true;
        }

        serviceScope = null;
        return false;
    }
}
