using Avalonia;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using FluentAvalonia.Core;
using System.Collections;
using Toolkit.UI.Controls.Avalonia;
using ISelectable = Toolkit.Foundation.ISelectable;

namespace Toolkit.UI.Avalonia;

public class NavigationViewItemInvokedBehaviour : Trigger
{
    public static readonly StyledProperty<bool> SelectsChildOnInvokedProperty =
        AvaloniaProperty.Register<NavigationViewItemInvokedBehaviour, bool>(nameof(SelectsChildOnInvoked));

    private NavigationView? navigationView;

    public event TypedEventHandler<NavigationViewItem, EventArgs>? Invoked;

    public bool SelectsChildOnInvoked
    {
        get => GetValue(SelectsChildOnInvokedProperty);
        set => SetValue(SelectsChildOnInvokedProperty, value);
    }

    protected override void OnAttached()
    {
        if (AssociatedObject is NavigationViewItem navigationViewItem)
        {
            navigationViewItem.AttachedToVisualTree += OnAttachedToVisualTree;
        }

        base.OnAttached();
    }

    protected override void OnDetachedFromVisualTree()
    {
        if (navigationView is not null)
        {
            navigationView.ItemInvoked -= OnItemInvoked;
        }

        base.OnDetachedFromVisualTree();
    }

    private void OnAttachedToVisualTree(object? sender,
        VisualTreeAttachmentEventArgs args)
    {
        if (AssociatedObject is NavigationViewItem navigationViewItem)
        {
            navigationViewItem.AttachedToVisualTree -= OnAttachedToVisualTree;
            SetupNavigationView();
        }
    }

    private void OnItemInvoked(object? sender,
        FluentAvalonia.UI.Controls.NavigationViewItemInvokedEventArgs args)
    {
        if (AssociatedObject is NavigationViewItem navigationViewItem)
        {
            if (args.InvokedItemContainer == AssociatedObject)
            {
                Interaction.ExecuteActions(AssociatedObject, Actions, null);
                if (!navigationViewItem.IsChildSelected && navigationViewItem.MenuItemsSource is IList collection)
                {
                    if (collection is { Count: > 0 } && collection[0] is ISelectable selectable)
                    {
                        selectable.Selected = true;
                    }
                }
            }
        }
    }

    private void SetupNavigationView()
    {
        if (AssociatedObject is NavigationViewItem navigationViewItem)
        {
            if (navigationViewItem.GetVisualAncestors().OfType<NavigationView>().FirstOrDefault() is NavigationView navigationView)
            {
                this.navigationView = navigationView;
                navigationView.ItemInvoked += OnItemInvoked;
            }
        }
    }
}
