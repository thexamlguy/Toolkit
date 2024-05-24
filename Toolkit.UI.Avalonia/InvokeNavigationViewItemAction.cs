using Avalonia;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using System.Collections;
using Toolkit.Foundation;
using Toolkit.UI.Controls.Avalonia;

namespace Toolkit.UI.Avalonia;

public class InvokeNavigationViewItemAction :
    AvaloniaObject,
    IAction
{
    public object? Execute(object? sender, object? parameter)
    {
        if (sender is NavigationViewItem navigationViewItem)
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (navigationViewItem.MenuItemsSource is IList collection)
                {
                    if (collection is { Count: > 0 } && collection[0] is ISelectable selectable)
                    {
                        selectable.Selected = true;
                    }
                }
            }, DispatcherPriority.ContextIdle);
        }

        if (sender is NavigationView navigationView)
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (navigationView.MenuItemsSource is IList collection)
                {
                    if (collection is { Count: > 0 } && collection[0] is ISelectable selectable)
                    {
                        selectable.Selected = true;
                    }
                }
            }, DispatcherPriority.ContextIdle);
        }

        return true;
    }
}