using Avalonia.Controls;
using Avalonia.Interactivity;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

public class ContentControlHandler :
    INotificationHandler<NavigateEventArgs<ContentControl>>
{
    public async Task Handle(NavigateEventArgs<ContentControl> args)
    {
        if (args.Region is ContentControl contentControl)
        {
            if (args.Template is Control control)
            {
                TaskCompletionSource taskCompletionSource = new();
                async void HandleLoaded(object? sender, RoutedEventArgs args)
                {
                    control.Loaded -= HandleLoaded;
                    if (control.DataContext is object content)
                    {
                        if (content is IActivated activated)
                        {
                            await activated.Activated();
                        }
                    }

                    taskCompletionSource.SetResult();
                }

                async void HandleUnloaded(object? sender, RoutedEventArgs args)
                {
                    control.Unloaded -= HandleLoaded;
                    if (control.DataContext is object content)
                    {
                        if (content is IDeactivated deactivated)
                        {
                            await deactivated.Deactivated();
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

                await taskCompletionSource.Task;
            }
        }
    }
}