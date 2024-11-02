namespace Toolkit.UI.Controls.Avalonia;

public class NavigationViewItem :
    FluentAvalonia.UI.Controls.NavigationViewItem
{
    protected override Type StyleKeyOverride =>
        typeof(FluentAvalonia.UI.Controls.NavigationViewItem);
}