using Avalonia;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using System.Collections;
using Toolkit.UI.Controls.Avalonia;

namespace Toolkit.UI.Avalonia;

public class InvokeNavigationViewItemAction :
    AvaloniaObject,
    IAction
{
    private int currentIndex;

    public static readonly StyledProperty<int> SelectedIndexProperty =
        AvaloniaProperty.Register<InvokeNavigationViewItemAction, int>(nameof(SelectedIndex), 0);

    public static readonly StyledProperty<object> TargetProperty =
        AvaloniaProperty.Register<InvokeNavigationViewItemAction, object>(nameof(Target));

    public object Target
    {
        get => GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }

    public int SelectedIndex
    {
        get => GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    public object? Execute(object? sender, object? parameter)
    {
        //if (SelectedIndex == currentIndex)
        //{
        //    return false;
        //}

        if ((Target ?? sender) is NavigationViewItem navigationViewItem)
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

        if ((Target ?? sender) is NavigationView navigationView)
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                if (navigationView.MenuItemsSource is IList collection)
                {
                    if (collection is { Count: > 0 })
                    {
                        navigationView.SetValue(NavigationView.SelectedItemProperty, collection[SelectedIndex]);
                        currentIndex = SelectedIndex;
                    }
                    else
                    {
                        navigationView.SetValue(NavigationView.SelectedItemProperty, null);
                    }
                }
            }, DispatcherPriority.ContextIdle);
        }

        return true;
    }
}