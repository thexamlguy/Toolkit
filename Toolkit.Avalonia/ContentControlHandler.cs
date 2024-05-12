using Avalonia.Controls;
using Avalonia.Interactivity;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

public class ContentControlHandler :
    INavigateHandler<ContentControl>
{
    public async Task Handle(NavigateEventArgs<ContentControl> args,
        CancellationToken cancellationToken)
    {
        if (args.Context is ContentControl contentControl)
        {
            if (args.Template is Control control)
            {
                TaskCompletionSource taskCompletionSource = new();
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

                    taskCompletionSource.SetResult();
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

                control.DataContext = args.Content;
                contentControl.Content = control;
                await taskCompletionSource.Task;
            }
        }
    }
}