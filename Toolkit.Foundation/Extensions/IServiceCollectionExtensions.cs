using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Toolkit.Foundation
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddHandler<TRequestHandler>(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAdd(new ServiceDescriptor(typeof(TRequestHandler), typeof(TRequestHandler), ServiceLifetime.Transient));
            return serviceCollection;
        }

        public static IServiceCollection AddFoundation(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IServiceFactory>(provider => new ServiceFactory(provider.GetService, (instanceType, parameters) => ActivatorUtilities.CreateInstance(provider, instanceType, parameters!)));

            return serviceCollection;
        }
    }
}
