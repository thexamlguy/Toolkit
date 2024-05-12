using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

public class ContentTemplate :
    IContentTemplate,
    IDataTemplate
{
    public Control? Build(object? item)
    {
        if (item is IObservableViewModel observableViewModel)
        {
            if (observableViewModel.Provider is IServiceProvider provider)
            {
                IContentTemplateDescriptorProvider? contentTemplateProvider = provider.GetService<IContentTemplateDescriptorProvider>();
                INavigationRegion? viewModelContentBinder = provider.GetService<INavigationRegion>();

                if (contentTemplateProvider?.Get(item.GetType().Name) is IContentTemplateDescriptor descriptor)
                {
                    if (provider.GetRequiredKeyedService(descriptor.TemplateType, descriptor.Key) is Control control)
                    {
                        async void HandleLoaded(object? sender, RoutedEventArgs args)
                        {
                            control.Loaded -= HandleLoaded;
                            if (control.DataContext is object content)
                            {
                                if (content is IInitializer initializer)
                                {
                                    await initializer.Initialize();
                                }

                                if (content is IActivated activated)
                                {
                                    await activated.OnActivated();
                                }
                            }
                        }

                        async void HandleUnloaded(object? sender, RoutedEventArgs args)
                        {
                            control.Unloaded -= HandleLoaded;
                            if (control.DataContext is object content)
                            {
                                if (content is IDeactivated deactivated)
                                {
                                    await deactivated.OnDeactivated();
                                }
                            }
                        }

                        control.Loaded += HandleLoaded;
                        control.Unloaded += HandleUnloaded;

                        //viewModelContentBinder?.Register(control);

                        return control;
                    }
                }
            }
        }

        return default;
    }

    public bool Match(object? data) => true;
}