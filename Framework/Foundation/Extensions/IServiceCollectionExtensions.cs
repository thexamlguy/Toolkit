using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Toolkit.Framework.Foundation;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddHandler<TRequestHandler>(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAdd(new ServiceDescriptor(typeof(TRequestHandler), typeof(TRequestHandler), ServiceLifetime.Transient));
        return serviceCollection;
    }

    public static IServiceCollection AddFoundation(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IServiceFactory>(provider => new ServiceFactory(provider.GetService, (instanceType, parameters) => ActivatorUtilities.CreateInstance(provider, instanceType, parameters!)))
            .AddSingleton<IInitialization, Initialization>(provider => new Initialization(() =>
            {
                return serviceCollection.Where(x => x.ServiceType.GetInterfaces()
                    .Contains(typeof(IInitializable)) || x.ServiceType == typeof(IInitializable))
                    .GroupBy(x => x.ServiceType)
                    .Select(x => x.First())
                    .SelectMany(x => provider.GetServices(x.ServiceType)
                    .Select(x => (IInitializable?)x)).ToList();
            }));

        return serviceCollection;
    }
}