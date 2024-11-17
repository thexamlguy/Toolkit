using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Toolkit.Foundation;

namespace Toolkit.WinUI;

[Bindable]
public class TemplateControl :
    ContentControl
{
    public TemplateControl()
    {
        DefaultStyleKey = typeof(TemplateControl);
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender,
        RoutedEventArgs args)
    {
        Loaded -= OnLoaded;

        if (DataContext is IObservableViewModel observableViewModel)
        {
            if (observableViewModel.Provider is IServiceProvider provider)
            {
                if (provider.GetRequiredKeyedService<IContentTemplateDescriptor>(DataContext.GetType().Name.Replace("ViewModel", ""))
                    is IContentTemplateDescriptor descriptor)
                {
                    if (provider.GetRequiredKeyedService(descriptor.TemplateType, descriptor.Key)
                        is FrameworkElement control)
                    {
                        void HandleLoaded(object? sender, RoutedEventArgs args)
                        {
                            control.Loaded -= HandleLoaded;
                            if (control.DataContext is object content)
                            {
                                if (content is IActivation activation)
                                {
                                    activation.IsActive = true;
                                }
                            }
                        }

                        void HandleDataContextChanged(FrameworkElement? sender, DataContextChangedEventArgs args)
                        {
                            if (control.DataContext is object content)
                            {
                                if (content is IActivation activation)
                                {
                                    activation.IsActive = true;
                                }
                            }
                        }

                        void HandleUnloaded(object? sender, RoutedEventArgs args)
                        {
                            control.Unloaded -= HandleUnloaded;
                            if (control.DataContext is object content)
                            {
                                if (content is IActivation activation)
                                {
                                    activation.IsActive = false;
                                }
                            }
                        }

                        control.Loaded += HandleLoaded;
                        control.Unloaded += HandleUnloaded;
                        control.DataContextChanged += HandleDataContextChanged;

                        Content = control;
                    }
                }
            }
        }
    }
}
