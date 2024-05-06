using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Toolkit.UI.Controls.Avalonia;

namespace Toolkit.UI.Avalonia;

public class ListBoxItemExtension
{
    public static readonly AttachedProperty<bool> IsItemClickEnabledProperty =
        AvaloniaProperty.RegisterAttached<ListBoxItem, bool>("IsItemClickEnabled",
            typeof(ListBoxItemExtension), false);

    public static readonly RoutedEvent<ItemInvokedEventArgs> ItemClickEvent =
        RoutedEvent.Register<ItemInvokedEventArgs>("ItemClick",
            RoutingStrategies.Bubble, typeof(ListBoxItemExtension));

    static ListBoxItemExtension()
    {
        IsItemClickEnabledProperty.Changed.AddClassHandler<ListBoxItem>(OnIsItemClickEnabledPropertyChanged);
    }

    private static void OnIsItemClickEnabledPropertyChanged(ListBoxItem sender,
        AvaloniaPropertyChangedEventArgs args)
    {
        bool TrySetupListBox()
        {
            if (sender.GetLogicalAncestors().OfType<ListBox>().FirstOrDefault() is ListBox listBox)
            {
                void OnItemInvoked(object? _, FluentAvalonia.UI.Controls.NavigationViewItemInvokedEventArgs args)
                {
                    if (args.InvokedItemContainer == sender)
                    {
                        sender.RaiseEvent(new ItemInvokedEventArgs { RoutedEvent = ItemClickEvent });
                    }
                }

                void OnSelectionChanged(object? sender, SelectionChangedEventArgs args)
                {
                    foreach (object item in args.AddedItems)
                    {
                        if (listBox.ContainerFromItem(item) == sender)
                        { 
                        
                        }
                    }
                }


                listBox.SelectionChanged += OnSelectionChanged;
                return true;
            }

            return false;
        }

        if (!TrySetupListBox())
        {
            void OnAttachedToVisualTree(object? _, VisualTreeAttachmentEventArgs __)
            {
                sender.AttachedToVisualTree -= OnAttachedToVisualTree;
                TrySetupListBox();
            }

            sender.AttachedToVisualTree += OnAttachedToVisualTree;
        }
    }

    public static bool GetIsItemClickEnabled(ListBoxItem element) =>
        element.GetValue(IsItemClickEnabledProperty);

    public static void SetIsItemClickEnabled(ListBoxItem element, bool value) =>
        element.SetValue(IsItemClickEnabledProperty, value);

    public static void AddItemClickHandler(ListBoxItem element, EventHandler<ItemInvokedEventArgs> handler) =>
        element.AddHandler(ItemClickEvent, handler);

    public static void RemoveItemClickHandler(ListBoxItem element, EventHandler<ItemInvokedEventArgs> handler) =>
        element.RemoveHandler(ItemClickEvent, handler);
}


public class NavigationViewItemExtension
{
    public static readonly AttachedProperty<bool> IsItemClickEnabledProperty =
        AvaloniaProperty.RegisterAttached<NavigationViewItem, bool>("IsItemClickEnabled",
            typeof(NavigationViewItemExtension), false);

    public static readonly RoutedEvent<ItemInvokedEventArgs> ItemClickEvent = 
        RoutedEvent.Register<ItemInvokedEventArgs>("ItemClick", 
            RoutingStrategies.Bubble, typeof(NavigationViewItemExtension));

    static NavigationViewItemExtension()
    {
        IsItemClickEnabledProperty.Changed.AddClassHandler<NavigationViewItem>(OnIsItemClickEnabledPropertyChanged);
    }

    private static void OnIsItemClickEnabledPropertyChanged(NavigationViewItem sender, 
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
                        sender.RaiseEvent(new ItemInvokedEventArgs { RoutedEvent = ItemClickEvent });
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

    public static bool GetIsItemClickEnabled(NavigationViewItem element) =>
        element.GetValue(IsItemClickEnabledProperty);

    public static void SetIsItemClickEnabled(NavigationViewItem element, bool value) => 
        element.SetValue(IsItemClickEnabledProperty, value);

    public static void AddItemClickHandler(NavigationViewItem element, EventHandler<ItemInvokedEventArgs> handler) => 
        element.AddHandler(ItemClickEvent, handler);

    public static void RemoveItemClickHandler(NavigationViewItem element, EventHandler<ItemInvokedEventArgs> handler) =>
        element.RemoveHandler(ItemClickEvent, handler);
}
