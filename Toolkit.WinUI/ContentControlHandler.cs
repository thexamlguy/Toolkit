using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Toolkit.Foundation;

namespace Toolkit.WinUI;

public class ContentControlHandler :
    IHandler<NavigateTemplateEventArgs>
{
    public void Handle(NavigateTemplateEventArgs args)
    {
        if (args.Region is not ContentControl contentControl)
        {
            return;
        }

        if (args.Template is not Control control)
        {
            return;
        }

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

        void HandleUnloaded(object? sender, RoutedEventArgs args)
        {
            control.Unloaded -= HandleLoaded;
            if (control.DataContext is object content)
            {
                if (content is IActivation activation)
                {
                    activation.IsActive = false;
                }

                if (content is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        control.Loaded += HandleLoaded;
        control.Unloaded += HandleUnloaded;

        control.DataContext = args.Content;
        contentControl.Content = control;
    }
}
