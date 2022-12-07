using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Toolkit.Foundation.Avalonia
{ 
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddNavigation(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<INavigationRouteDescriptorCollection, NavigationRouteDescriptorCollection>();
            serviceCollection.TryAddSingleton<INavigationRouter, NavigationRouter>();

            return serviceCollection;
        }
    }
}
