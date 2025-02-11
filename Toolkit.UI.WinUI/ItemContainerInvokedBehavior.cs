using Microsoft.UI.Xaml;
using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.Xaml.Interactivity;

namespace Toolkit.UI.WinUI;

public class ItemContainerInvokedBehavior : 
    Trigger<ItemContainer>
{
    private ItemsView? itemsView;

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is not null)
        {
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnloaded;
        }
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        if (AssociatedObject is not null)
        {
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.Unloaded -= OnUnloaded;
        }
        DetachFromItemsView();
    }

    private void OnLoaded(object sender, RoutedEventArgs args)
    {
        if (AssociatedObject is not null)
        {
            itemsView = FindAncestor<ItemsView>(AssociatedObject);
            if (itemsView is not null)
            {
                itemsView.SelectionChanged += OnSelectionChanged;
            }
        }
    }

    private void OnUnloaded(object sender, RoutedEventArgs args)
    {
        DetachFromItemsView();
        if (AssociatedObject is not null)
        {
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.Unloaded -= OnUnloaded;
        }
    }

    private void DetachFromItemsView()
    {
        if (itemsView is not null)
        {
            itemsView.SelectionChanged -= OnSelectionChanged;
            itemsView = null;
        }
    }

    private static T? FindAncestor<T>(DependencyObject current) where T : DependencyObject
    {
        while (current != null)
        {
            if (current is T ancestor)
            {
                return ancestor;
            }
            current = VisualTreeHelper.GetParent(current);
        }
        return null;
    }

    private void OnSelectionChanged(ItemsView sender, ItemsViewSelectionChangedEventArgs args)
    {
        if (itemsView is not null && AssociatedObject is not null && AssociatedObject.DataContext == itemsView.SelectedItem)
        {
            Interaction.ExecuteActions(AssociatedObject, Actions, null);
        }
    }
}