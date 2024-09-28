using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;

namespace Toolkit.UI.Avalonia;

public class TabStripExtension
{
    public static readonly AttachedProperty<bool> IsItemInvokedEnabledProperty =
        AvaloniaProperty.RegisterAttached<TabStripItem, bool>("IsItemInvokedEnabled",
            typeof(TabStripExtension), false);

    public static readonly RoutedEvent<ItemInvokedEventArgs> ItemInvokedEvent =
        RoutedEvent.Register<ItemInvokedEventArgs>("ItemInvoked",
            RoutingStrategies.Bubble, typeof(TabStripExtension));

    static TabStripExtension()
    {
        IsItemInvokedEnabledProperty.Changed.AddClassHandler<TabStripItem>(OnIsItemClickEnabledPropertyChanged);
    }

    private static void OnIsItemClickEnabledPropertyChanged(TabStripItem sender,
        AvaloniaPropertyChangedEventArgs args)
    {
        bool TrySetupTabStrip()
        {
            if (sender.GetLogicalAncestors().OfType<TabStrip>().FirstOrDefault() is TabStrip tabStrip)
            {
                void OnItemInvoked(object? _, SelectionChangedEventArgs args)
                {
                    if (args.AddedItems is { Count: > 0 })
                    {
                        if (sender.DataContext == tabStrip.SelectedItem)
                        {
                            sender.RaiseEvent(new ItemInvokedEventArgs { RoutedEvent = ItemInvokedEvent });
                        }
                    }
                }

                if (sender.DataContext == tabStrip.SelectedItem)
                {
                    sender.RaiseEvent(new ItemInvokedEventArgs { RoutedEvent = ItemInvokedEvent });
                }

                void HandleUnloaded(object? _, RoutedEventArgs __)
                {
                    tabStrip.SelectionChanged -= OnItemInvoked;
                    tabStrip.Unloaded -= HandleUnloaded;
                }

                tabStrip.SelectionChanged += OnItemInvoked;
                tabStrip.Unloaded += HandleUnloaded;

                return true;
            }

            return false;
        }

        if (!TrySetupTabStrip())
        {
            void HandleLoaded(object? _, RoutedEventArgs __)
            {
                TrySetupTabStrip();
            }

            sender.Loaded += HandleLoaded;
        }
    }

    public static bool GetIsItemInvokedEnabled(TabStripItem element) =>
        element.GetValue(IsItemInvokedEnabledProperty);

    public static void SetIsItemInvokedEnabled(TabStripItem element, bool value) =>
        element.SetValue(IsItemInvokedEnabledProperty, value);

    public static void AddItemInvokedHandler(TabStripItem element, EventHandler<ItemInvokedEventArgs> handler) =>
        element.AddHandler(ItemInvokedEvent, handler);

    public static void RemoveItemInvokedHandler(TabStripItem element, EventHandler<ItemInvokedEventArgs> handler) =>
        element.RemoveHandler(ItemInvokedEvent, handler);
}
