using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public static class IServiceProviderExtensions
{
    public static object GetRequiredKeyedService(this IServiceProvider provider, 
        Type serviceType, 
        Action<object> serviceDelegate, 
        object? serviceKey)
    {
        object service = provider.GetRequiredKeyedService(serviceType, serviceKey);
        serviceDelegate.Invoke(service);

        return service;
    }
}
