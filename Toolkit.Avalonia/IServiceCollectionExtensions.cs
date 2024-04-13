using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddConfigurationTemplate<TConfiguration, TValue, THeader,
        TDescription, TAction>(this IServiceCollection services,
        params object[]? parameters)
        where TConfiguration : class
        where THeader : class
        where TDescription : class
        where TAction : class
    {
        Type viewModelType = typeof(ComponentConfigurationViewModel<TConfiguration, TValue, THeader, TDescription, TAction>);
        Type viewType = typeof(Button);

        object key = viewModelType.Name.Replace("ViewModel", "");

        services.AddTransient<IComponentConfigurationViewModel,
            ComponentConfigurationViewModel<TConfiguration, TValue, THeader, TDescription, TAction>>(provider =>
            provider.GetRequiredService<IServiceFactory>()
                .Create<ComponentConfigurationViewModel<TConfiguration, TValue, THeader, TDescription, TAction>>(parameters)!);

        services.TryAddTransient(viewType);

        services.AddKeyedTransient<IComponentConfigurationViewModel,
            ComponentConfigurationViewModel<TConfiguration, TValue, THeader, TDescription, TAction>>(key, (provider, key) =>
            provider.GetRequiredService<IServiceFactory>()
                .Create<ComponentConfigurationViewModel<TConfiguration, TValue, THeader, TDescription, TAction>>(parameters)!);

        services.TryAddKeyedTransient(viewType, key);

        services.AddTransient<IContentTemplateDescriptor>(provider =>
            new ContentTemplateDescriptor(key, viewModelType, viewType, parameters));

        services.TryAddTransient<THeader>();
        services.TryAddTransient<TDescription>();
        services.TryAddTransient<TAction>();

        return services;
    }

    public static IServiceCollection AddConfigurationTemplate<TConfiguration, TValue, TAction>(this IServiceCollection services,
        Func<TConfiguration, TValue> valueDelegate,
        object header,
        object description,
        params object[]? parameters)
        where TConfiguration : class
        where TAction : class
    {
        Type viewModelType = typeof(ComponentConfigurationViewModel<TConfiguration, TValue, TAction>);
        Type viewType = typeof(Button);

        object key = viewModelType.Name.Replace("ViewModel", "");

        parameters = [valueDelegate, header, description, .. parameters ?? Enumerable.Empty<object?>()];

        services.AddTransient<IComponentConfigurationViewModel,
            ComponentConfigurationViewModel<TConfiguration, TValue, TAction>>(provider =>
            provider.GetRequiredService<IServiceFactory>()
                .Create<ComponentConfigurationViewModel<TConfiguration, TValue, TAction>>(parameters)!);

        services.TryAddTransient(viewType);

        services.AddKeyedTransient<IComponentConfigurationViewModel,
            ComponentConfigurationViewModel<TConfiguration, TValue, TAction>>(key, (provider, key) =>
            provider.GetRequiredService<IServiceFactory>()
                .Create<ComponentConfigurationViewModel<TConfiguration, TValue, TAction>>(parameters)!);

        services.TryAddKeyedTransient(viewType, key);

        services.AddTransient<IContentTemplateDescriptor>(provider =>
            new ContentTemplateDescriptor(key, viewModelType, viewType, parameters));

        services.TryAddTransient<TAction>();

        return services;
    }

    public static IServiceCollection AddConfigurationTemplate<TConfiguration, TValue,
        TDescription, TAction>(this IServiceCollection services,
        Func<TConfiguration, TValue> valueDelegate,
        object description,
        params object[]? parameters)
        where TConfiguration : class
        where TDescription : class
        where TAction : class
    {
        Type viewModelType = typeof(ComponentConfigurationViewModel<TConfiguration, TValue, TDescription, TAction>);
        Type viewType = typeof(Button);

        object key = viewModelType.Name.Replace("ViewModel", "");

        parameters = [valueDelegate, description, .. parameters ?? Enumerable.Empty<object?>()];

        services.AddTransient<IComponentConfigurationViewModel,
            ComponentConfigurationViewModel<TConfiguration, TValue, TDescription, TAction>>(provider =>
            provider.GetRequiredService<IServiceFactory>()
                .Create<ComponentConfigurationViewModel<TConfiguration, TValue, TDescription, TAction>>(parameters)!);

        services.TryAddTransient(viewType);

        services.AddKeyedTransient<IComponentConfigurationViewModel,
            ComponentConfigurationViewModel<TConfiguration, TValue, TDescription, TAction>>(key, (provider, key) =>
            provider.GetRequiredService<IServiceFactory>()
                .Create<ComponentConfigurationViewModel<TConfiguration, TValue, TDescription, TAction>>(parameters)!);

        services.TryAddKeyedTransient(viewType, key);

        services.AddTransient<IContentTemplateDescriptor>(provider =>
            new ContentTemplateDescriptor(key, viewModelType, viewType, parameters));

        services.TryAddTransient<TDescription>();
        services.TryAddTransient<TAction>();

        return services;
    }

    public static IServiceCollection AddAvalonia(this IServiceCollection services)
    {
        services.AddTransient<IDispatcher, AvaloniaDispatcher>();

        services.AddTransient<IContentTemplate, ContentTemplate>();
        services.AddTransient<INavigationContext, NavigationContext>();

        services.AddNavigateHandler<ClassicDesktopStyleApplicationHandler>();
        services.AddNavigateHandler<SingleViewApplicationHandler>();
        services.AddNavigateHandler<ContentControlHandler>();
        services.AddNavigateHandler<FrameHandler>();
        services.AddNavigateHandler<ContentDialogHandler>();

        services.AddScoped<INavigationContextCollection, NavigationContextCollection>(provider => new NavigationContextCollection
        {
            { typeof(IClassicDesktopStyleApplicationLifetime), typeof(IClassicDesktopStyleApplicationLifetime) },
            { typeof(ISingleViewApplicationLifetime), typeof(ISingleViewApplicationLifetime) }
        });

        services.AddTransient((Func<IServiceProvider, IProxyServiceCollection<IComponentBuilder>>)(provider =>
            new ProxyServiceCollection<IComponentBuilder>(services =>
            {
                services.AddSingleton(provider.GetRequiredService<IDispatcher>());

                services.AddTransient<IContentTemplateDescriptorProvider, ContentTemplateDescriptorProvider>();
                services.AddTransient<IContentTemplate, ContentTemplate>();

                services.AddTransient<INavigationContext, NavigationContext>();

                services.AddNavigateHandler<ContentControlHandler>();
                services.AddNavigateHandler<FrameHandler>();
                services.AddNavigateHandler<ContentDialogHandler>();
            })));

        return services;
    }
}