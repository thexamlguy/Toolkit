using Avalonia.Controls;
using Avalonia.Interactivity;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

public class ContentControlHandler :
    IHandler<NavigateEventArgs<ContentControl>>
{
    public void Handle(NavigateEventArgs<ContentControl> args)
    {
        if (args.Region is ContentControl contentControl)
        {
            if (args.Template is Control control)
            {
                TaskCompletionSource taskCompletionSource = new();
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

                    taskCompletionSource.SetResult();
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

                contentControl.Content = null;
                contentControl.Content = control;
            }
        }
    }
}