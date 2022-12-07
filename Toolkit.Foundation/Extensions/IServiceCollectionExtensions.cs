using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddFoundation(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IServiceFactory>(provider => new ServiceFactory(provider.GetService, (instanceType, parameters) => ActivatorUtilities.CreateInstance(provider, instanceType, parameters!)));

            serviceCollection
                .AddSingleton<IInitialization, Initialization>(provider => new Initialization(() =>
                {
                    return serviceCollection.Where(x => x.ServiceType.GetInterfaces()
                        .Contains(typeof(IInitializer)) || x.ServiceType == typeof(IInitializer))
                        .GroupBy(x => x.ServiceType)
                        .Select(x => x.First())
                        .SelectMany(x => provider.GetServices(x.ServiceType)
                        .Select(x => (IInitializer?)x)).ToList();
                }));

            return serviceCollection;
        }
    }
}
