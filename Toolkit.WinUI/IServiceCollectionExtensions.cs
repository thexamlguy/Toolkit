using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Toolkit.Foundation;

namespace Toolkit.WinUI;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddWinUI(this IServiceCollection services)
    {
        services.AddTransient<IDispatcher, WinUIDispatcher>();
        services.AddTransient<IDispatcherTimerFactory, DispatcherTimerFactory>();
        services.AddSingleton<IWindowRegistry, WindowRegistry>();

        services.AddTransient<IContentTemplate, ContentTemplate>();
        services.AddTransient<INavigationRegion, NavigationRegion>();

        services.AddHandler<NavigateTemplateEventArgs, ContentControlHandler>(nameof(ContentControl));
        services.AddHandler<NavigateTemplateEventArgs, ContentDialogHandler>(nameof(ContentDialog));

        services.AddTransient((Func<IServiceProvider, IProxyServiceCollection<IComponentBuilder>>)(provider =>
            new ProxyServiceCollection<IComponentBuilder>(services =>
            {

            })));

        return services;
    }
}