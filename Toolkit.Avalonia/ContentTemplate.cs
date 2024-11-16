using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using Toolkit.Foundation;
using Toolkit.UI.Controls.Avalonia;

namespace Toolkit.Avalonia;

public class ContentTemplate :
    IContentTemplate,
    IDataTemplate,
    IItemContainerTemplateSelector
{
    public Control? Build(object? item)
    {
        if (item is IObservableViewModel observableViewModel)
        {
            if (observableViewModel.Provider is IServiceProvider provider)
            {
                Type itemType = item.GetType();
                if (provider.GetRequiredKeyedService<IContentTemplateDescriptor>(itemType.Name.Replace("ViewModel", ""))
                    is IContentTemplateDescriptor descriptor)
                {
                    if (provider.GetRequiredKeyedService(descriptor.TemplateType, descriptor.Key) is Control control)
                    {
                        async void HandleLoaded(object? sender, RoutedEventArgs args)
                        {
                            control.Loaded -= HandleLoaded;
                            if (control.DataContext is object content)
                            {
                                //if (content is IActivated activated)
                                //{
                                //    await activated.OnActivated();
                                //}
                            }
                        }

                        async void HandleDataContextChanged(object? sender, EventArgs args)
                        {
                            if (control.DataContext is object content)
                            {
                                //if (content is IActivated activated)
                                //{
                                //    await activated.OnActivated();
                                //}
                            }
                        }

                        async void HandleUnloaded(object? sender, RoutedEventArgs args)
                        {
                            control.Unloaded -= HandleUnloaded;
                            if (control.DataContext is object content)
                            {
                                //if (content is IDeactivated deactivated)
                                //{
                                //    await deactivated.OnDeactivated();
                                //}
                            }
                        }

                        control.Loaded += HandleLoaded;
                        control.Unloaded += HandleUnloaded;
                        control.DataContextChanged += HandleDataContextChanged; ;

                        return control;
                    }
                }
            }
        }

        return default;
    }

    public bool Match(object? data) => true;

    public IDataTemplate? SelectTemplate(object? item, ItemsControl itemsControl)
    {
        if (item is IObservableViewModel observableViewModel)
        {
            if (observableViewModel.Provider is IServiceProvider provider)
            {
                Type itemType = item.GetType();
                if (provider.GetRequiredKeyedService<IContentTemplateDescriptor>(itemType.Name.Replace("ViewModel", ""))
                    is IContentTemplateDescriptor descriptor)
                {
                    if (provider.GetRequiredKeyedService(descriptor.TemplateType, descriptor.Key) is Control control)
                    {
                        async void HandleLoaded(object? sender, RoutedEventArgs args)
                        {
                            control.Loaded -= HandleLoaded;
                            if (control.DataContext is object content)
                            {
                                //if (content is IActivated activated)
                                //{
                                //    await activated.OnActivated();
                                //}
                            }
                        }

                        async void HandleDataContextChanged(object? sender, EventArgs args)
                        {
                            if (control.DataContext is object content)
                            {
                                //if (content is IActivated activated)
                                //{
                                //    await activated.OnActivated();
                                //}
                            }
                        }

                        async void HandleUnloaded(object? sender, RoutedEventArgs args)
                        {
                            control.Unloaded -= HandleUnloaded;
                            if (control.DataContext is object content)
                            {
                                //if (content is IDeactivated deactivated)
                                //{
                                //    await deactivated.OnDeactivated();
                                //}
                            }
                        }

                        control.Loaded += HandleLoaded;
                        control.Unloaded += HandleUnloaded;
                        control.DataContextChanged += HandleDataContextChanged; ;

                        return new FuncDataTemplate(item.GetType(), (_, _) => control);
                    }
                }
            }
        }

        return default;
    }
}