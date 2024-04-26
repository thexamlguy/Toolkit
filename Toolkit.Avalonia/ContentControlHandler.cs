using Avalonia.Controls;
using Avalonia.Interactivity;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

public class ContentControlHandler(INavigationContext navigationContext) :
    INavigateHandler<ContentControl>
{
    public async Task Handle(Navigate<ContentControl> args,
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
                    }
                }

                control.Loaded += HandleLoaded;
                control.Unloaded += HandleUnloaded;

                control.DataContext = args.Content;
                contentControl.Content = control;

                navigationContext.Set(control);
                await taskCompletionSource.Task;
            }
        }
    }
}