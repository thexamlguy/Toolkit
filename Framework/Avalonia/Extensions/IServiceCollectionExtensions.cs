using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Toolkit.Framework.Foundation;

namespace Toolkit.Foundation.Avalonia;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddNavigation(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddSingleton<INavigationRouteDescriptorCollection, NavigationRouteDescriptorCollection>();

        serviceCollection.TryAddTransient<Navigation<Frame>, FrameNavigation>();
        serviceCollection.TryAddTransient<Navigation<ContentDialog>, ContentDialogNavigation>();
        serviceCollection.TryAddTransient<Navigation<ContentControl>, ContentControlNavigation>();

        return serviceCollection;
    }
}