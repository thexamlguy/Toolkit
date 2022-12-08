using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddFoundation(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IServiceFactory>(provider => new ServiceFactory(provider.GetService, (instanceType, parameters) => ActivatorUtilities.CreateInstance(provider, instanceType, parameters!)));

            return serviceCollection;
        }
    }
}
