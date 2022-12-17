using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using Avalonia.VisualTree;
using FluentAvalonia.UI.Controls;

namespace Toolkit.Controls.Avalonia;

public class NavigationViewItem : FluentAvalonia.UI.Controls.NavigationViewItem, IStyleable
{
    private NavigationView? navigationView;

    public event EventHandler<NavigationViewItemInvokedEventArgs>? Invoked;

    Type IStyleable.StyleKey => typeof(FluentAvalonia.UI.Controls.NavigationViewItem);

    protected override void OnApplyTemplate(TemplateAppliedEventArgs args)
    {
        navigationView = this.FindAncestorOfType<NavigationView>();
        if (navigationView is not null)
        {
            navigationView.ItemInvoked += OnItemInvoked;
        }

        base.OnApplyTemplate(args);
    }

    private void OnItemInvoked(object? sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.InvokedItemContainer == this)
        {
            Invoked?.Invoke(this, args);
        }
    }
}
