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
                void OnItemInvoked(object? _, FluentAvalonia.UI.Controls.NavigationViewItemInvokedEventArgs args)
                {
                    if (args.InvokedItemContainer == sender)
                    {
                        sender.RaiseEvent(new ItemInvokedEventArgs { RoutedEvent = ItemInvokedEvent });
                    }
                }

                navigationView.ItemInvoked += OnItemInvoked;
                return true;
            }

            return false;
        }

        if (!TrySetupNavigationView())
        {
            void OnAttachedToVisualTree(object? _, VisualTreeAttachmentEventArgs __)
            {
                sender.AttachedToVisualTree -= OnAttachedToVisualTree;
                TrySetupNavigationView();
            }

            sender.AttachedToVisualTree += OnAttachedToVisualTree;
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
