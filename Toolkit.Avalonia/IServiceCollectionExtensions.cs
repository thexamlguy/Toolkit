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
        services.AddTransient<IFilePicker, FilePicker>();
        services.AddTransient<IFolderPicker, FolderPicker>();

        services.AddTransient<IClipboardWriter, ClipboardWriter>();

        services.AddTransient<IImageReader, ImageReader>();
        services.AddTransient<IImageWriter, ImageWriter>();
        services.AddTransient<IImageResizer, ImageResizer>();

        services.AddTransient<IDispatcher, AvaloniaDispatcher>();

        services.AddTransient<IContentTemplate, ContentTemplate>();
        services.AddTransient<INavigationRegion, NavigationRegion>();

        services.AddAsyncHandler<WriteEventArgs<Clipboard<object>>, WriteClipboardHandler>();
        services.AddAsyncHandler<SelectionEventArgs<FolderPickerPicker>, IReadOnlyCollection<string>?, SelectFoldersHandler>();
        services.AddAsyncHandler<SelectionEventArgs<FilePickerFilter>, IReadOnlyCollection<string>?, SelectFilesHandler>();

        services.AddHandler<NavigateTemplateEventArgs, ClassicDesktopStyleApplicationHandler>(nameof(IClassicDesktopStyleApplicationLifetime));
        services.AddHandler<NavigateTemplateEventArgs, SingleViewApplicationHandler>(nameof(ISingleViewApplicationLifetime));

        services.AddHandler<NavigateTemplateEventArgs, ContentControlHandler>(nameof(ContentControl));

        services.AddHandler<NavigateTemplateEventArgs, FrameHandler>(nameof(Frame));
        services.TryAddSingleton<ITransientNavigationStore<Frame>, TransientNavigationStore<Frame>>();

        services.AddHandler<NavigateTemplateEventArgs, ContentDialogHandler>(nameof(ContentDialog));
        services.AddHandler<NavigateTemplateEventArgs, TaskDialogHandler>(nameof(TaskDialog));

        services.AddScoped<INavigationRegionCollection, NavigationRegionCollection>(provider => new NavigationRegionCollection
        {
            { typeof(IClassicDesktopStyleApplicationLifetime), typeof(IClassicDesktopStyleApplicationLifetime) },
            { typeof(ISingleViewApplicationLifetime), typeof(ISingleViewApplicationLifetime) }
        });

        services.AddTransient((Func<IServiceProvider, IProxyServiceCollection<IComponentBuilder>>)(provider =>
            new ProxyServiceCollection<IComponentBuilder>(services =>
            {
                services.AddTransient<ITopLevelProvider, TopLevelProvider>();
                services.AddTransient<IFilePicker, FilePicker>();
                services.AddTransient<IFolderPicker, FolderPicker>();

                services.AddTransient<IClipboardWriter, ClipboardWriter>();

                services.AddTransient<IImageReader, ImageReader>();
                services.AddTransient<IImageWriter, ImageWriter>();
                services.AddTransient<IImageResizer, ImageResizer>();

                services.AddSingleton(provider.GetRequiredService<IDispatcher>());
                services.AddTransient<IContentTemplate, ContentTemplate>();

                services.AddTransient<INavigationRegion, NavigationRegion>();

                services.AddAsyncHandler<WriteEventArgs<Clipboard<object>>, WriteClipboardHandler>();
                services.AddAsyncHandler<SelectionEventArgs<FolderPickerPicker>, IReadOnlyCollection<string>?, SelectFoldersHandler>();
                services.AddAsyncHandler<SelectionEventArgs<FilePickerFilter>, IReadOnlyCollection<string>?, SelectFilesHandler>();

                services.AddHandler<NavigateTemplateEventArgs, ContentControlHandler>(nameof(ContentControl));

                services.AddHandler<NavigateTemplateEventArgs, FrameHandler>(nameof(Frame));
                services.TryAddSingleton<ITransientNavigationStore<Frame>, TransientNavigationStore<Frame>>();

                services.AddHandler<NavigateTemplateEventArgs, ContentDialogHandler>(nameof(ContentDialog));
                services.AddHandler<NavigateTemplateEventArgs, TaskDialogHandler>(nameof(TaskDialog));
            })));

        return services;
    }
}