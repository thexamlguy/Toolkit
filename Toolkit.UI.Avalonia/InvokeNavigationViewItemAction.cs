using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using System.Collections;
using Toolkit.UI.Controls.Avalonia;

namespace Toolkit.UI.Avalonia;

public class InvokeNavigationViewItemAction :
    AvaloniaObject,
    IAction
{
    public static readonly StyledProperty<int> SelectedIndexProperty =
        AvaloniaProperty.Register<InvokeNavigationViewItemAction, int>(nameof(SelectedIndex), 0);

    public int SelectedIndex
    {
        get => GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    public object? Execute(object? sender, object? parameter)
    {
        if (sender is NavigationViewItem navigationViewItem)
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (navigationViewItem.MenuItemsSource is IList collection)
                {
                    if (collection is { Count: > 0 })
                    {
                        navigationViewItem.SetValue(NavigationView.SelectedItemProperty, collection[SelectedIndex]);
                    }
                }
            }, DispatcherPriority.ContextIdle);
        }

        if (sender is NavigationView navigationView)
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                if (navigationView.MenuItemsSource is IList collection)
                {
                    if (collection is { Count: > 0 })
                    {
                        if (collection[SelectedIndex] is ISelectable selectable)
                        {
                            selectable.IsSelected = true;
                        }

                        navigationView.SetValue(NavigationView.SelectedItemProperty, collection[SelectedIndex]);
                    }
                }
            }, DispatcherPriority.ContextIdle);
        }

        return true;
    }

}