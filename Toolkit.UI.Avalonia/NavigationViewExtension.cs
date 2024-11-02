using Avalonia;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Toolkit.UI.Controls.Avalonia;

namespace Toolkit.UI.Avalonia;

public class NavigationViewExtension
{
    public static readonly AttachedProperty<bool> IsItemInvokedEnabledProperty =
        AvaloniaProperty.RegisterAttached<NavigationViewItem, bool>("IsItemInvokedEnabled",
            typeof(NavigationViewExtension), false);

    public static readonly RoutedEvent<ItemInvokedEventArgs> ItemInvokedEvent =
        RoutedEvent.Register<ItemInvokedEventArgs>("ItemInvoked",
            RoutingStrategies.Bubble, typeof(NavigationViewExtension));

    static NavigationViewExtension()
    {
        IsItemInvokedEnabledProperty.Changed.AddClassHandler<NavigationViewItem>(OnIsItemInvokedEnabledPropertyChanged);
    }

    private static void OnIsItemInvokedEnabledPropertyChanged(NavigationViewItem sender,
        AvaloniaPropertyChangedEventArgs args)
    {
        bool TrySetupNavigationView()
        {
            if (sender.GetLogicalAncestors().OfType<NavigationView>().FirstOrDefault() is NavigationView navigationView)
            {
                sender.GetObservable(NavigationViewItem.IsSelectedProperty).Subscribe(args =>
                {
                    if (args)
                    {
                        sender.RaiseEvent(new ItemInvokedEventArgs { RoutedEvent = ItemInvokedEvent });
                    }
                });

                return true;
            }

            return false;
        }

        if (!TrySetupNavigationView())
        {
            void HandleLoaded(object? _, RoutedEventArgs __)
            {
                TrySetupNavigationView();
            }

            sender.Loaded += HandleLoaded;
        }
    }

    public static bool GetIsItemInvokedEnabled(NavigationViewItem element) =>
        element.GetValue(IsItemInvokedEnabledProperty);

    public static void SetIsItemInvokedEnabled(NavigationViewItem element, bool value) =>
        element.SetValue(IsItemInvokedEnabledProperty, value);

    public static void AddItemInvokedHandler(NavigationViewItem element, EventHandler<ItemInvokedEventArgs> handler) =>
        element.AddHandler(ItemInvokedEvent, handler);

    public static void RemoveItemInvokedHandler(NavigationViewItem element, EventHandler<ItemInvokedEventArgs> handler) =>
        element.RemoveHandler(ItemInvokedEvent, handler);
}