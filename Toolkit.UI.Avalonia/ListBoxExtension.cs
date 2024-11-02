using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;

namespace Toolkit.UI.Avalonia;

public class ListBoxExtension
{
    public static readonly AttachedProperty<bool> IsItemInvokedEnabledProperty =
        AvaloniaProperty.RegisterAttached<ListBoxItem, bool>("IsItemInvokedEnabled",
            typeof(ListBoxExtension), false);

    public static readonly RoutedEvent<ItemInvokedEventArgs> ItemInvokedEvent =
        RoutedEvent.Register<ItemInvokedEventArgs>("ItemInvoked",
            RoutingStrategies.Bubble, typeof(ListBoxExtension));

    static ListBoxExtension()
    {
        IsItemInvokedEnabledProperty.Changed.AddClassHandler<ListBoxItem>(OnIsItemClickEnabledPropertyChanged);
    }

    private static void OnIsItemClickEnabledPropertyChanged(ListBoxItem sender,
        AvaloniaPropertyChangedEventArgs args)
    {
        bool TrySetupListBox()
        {
            if (sender.GetLogicalAncestors().OfType<ListBox>().FirstOrDefault() is ListBox listBox)
            {
                void OnItemInvoked(object? _, SelectionChangedEventArgs args)
                {
                    if (args.AddedItems is { Count: > 0 })
                    {
                        if (sender.DataContext == listBox.SelectedItem)
                        {
                            sender.RaiseEvent(new ItemInvokedEventArgs { RoutedEvent = ItemInvokedEvent });
                        }
                    }
                }

                if (sender.DataContext == listBox.SelectedItem)
                {
                    sender.RaiseEvent(new ItemInvokedEventArgs { RoutedEvent = ItemInvokedEvent });
                }

                void HandleUnloaded(object? _, RoutedEventArgs __)
                {
                    listBox.SelectionChanged -= OnItemInvoked;
                    listBox.Unloaded -= HandleUnloaded;
                }

                listBox.SelectionChanged += OnItemInvoked;
                listBox.Unloaded += HandleUnloaded;

                return true;
            }

            return false;
        }

        if (!TrySetupListBox())
        {
            void HandleLoaded(object? _, RoutedEventArgs __)
            {
                TrySetupListBox();
            }

            sender.Loaded += HandleLoaded;
        }
    }

    public static bool GetIsItemInvokedEnabled(ListBoxItem element) =>
        element.GetValue(IsItemInvokedEnabledProperty);

    public static void SetIsItemInvokedEnabled(ListBoxItem element, bool value) =>
        element.SetValue(IsItemInvokedEnabledProperty, value);

    public static void AddItemInvokedHandler(ListBoxItem element, EventHandler<ItemInvokedEventArgs> handler) =>
        element.AddHandler(ItemInvokedEvent, handler);

    public static void RemoveItemInvokedHandler(ListBoxItem element, EventHandler<ItemInvokedEventArgs> handler) =>
        element.RemoveHandler(ItemInvokedEvent, handler);
}