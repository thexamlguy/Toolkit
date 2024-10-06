using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Toolkit.Foundation;
using Toolkit.UI.Controls.Avalonia;

namespace Toolkit.Avalonia;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddAvalonia(this IServiceCollection services)
    {
        services.AddTransient<ITopLevelProvider, TopLevelProvider>();
        services.AddTransient<IFileProvider, FileProvider>();
        services.AddTransient<IFolderProvider, FolderProvider>();

        services.AddTransient<IClipboardWriter, ClipboardWriter>();

        services.AddTransient<IImageReader, ImageReader>();
        services.AddTransient<IImageWriter, ImageWriter>();
        services.AddTransient<IImageResizer, ImageResizer>();

        services.AddTransient<IDispatcher, AvaloniaDispatcher>();

        services.AddTransient<IContentTemplate, ContentTemplate>();
        services.AddTransient<INavigationRegion, NavigationRegion>();

        services.AddHandler<WriteClipboardHandler>();
        services.AddHandler<SelectFoldersHandler>();
        services.AddHandler<SelectFilesHandler>();

        services.AddHandler<ClassicDesktopStyleApplicationHandler>(nameof(IClassicDesktopStyleApplicationLifetime));
        services.AddHandler<SingleViewApplicationHandler>(nameof(ISingleViewApplicationLifetime));
        services.AddHandler<ContentControlHandler>(nameof(ContentControl));
        services.AddHandler<FrameHandler>(nameof(Frame));
        services.AddHandler<ContentDialogHandler>(nameof(ContentDialog));
        services.AddHandler<TaskDialogHandler>(nameof(TaskDialog));

        services.AddScoped<INavigationRegionCollection, NavigationRegionCollection>(provider => new NavigationRegionCollection
        {
            { typeof(IClassicDesktopStyleApplicationLifetime), typeof(IClassicDesktopStyleApplicationLifetime) },
            { typeof(ISingleViewApplicationLifetime), typeof(ISingleViewApplicationLifetime) }
        });

        services.AddTransient((Func<IServiceProvider, IProxyServiceCollection<IComponentBuilder>>)(provider =>
            new ProxyServiceCollection<IComponentBuilder>(services =>
            {
                services.AddTransient<ITopLevelProvider, TopLevelProvider>();
                services.AddTransient<IFileProvider, FileProvider>();
                services.AddTransient<IFolderProvider, FolderProvider>();

                services.AddTransient<IClipboardWriter, ClipboardWriter>();

                services.AddTransient<IImageReader, ImageReader>();
                services.AddTransient<IImageWriter, ImageWriter>();
                services.AddTransient<IImageResizer, ImageResizer>();

                services.AddSingleton(provider.GetRequiredService<IDispatcher>());
                services.AddTransient<IContentTemplate, ContentTemplate>();

                services.AddTransient<INavigationRegion, NavigationRegion>();

                services.AddHandler<WriteClipboardHandler>();
                services.AddHandler<SelectFoldersHandler>();
                services.AddHandler<SelectFilesHandler>();

                services.AddHandler<ContentControlHandler>(nameof(ContentControl));
                services.AddHandler<FrameHandler>(nameof(Frame));
                services.AddHandler<ContentDialogHandler>(nameof(ContentDialog));
                services.AddHandler<TaskDialogHandler>(nameof(TaskDialog));
            })));

        return services;
    }
}