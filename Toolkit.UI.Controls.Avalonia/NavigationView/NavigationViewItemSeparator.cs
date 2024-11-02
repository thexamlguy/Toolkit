namespace Toolkit.UI.Controls.Avalonia;

public class NavigationViewItemSeparator :
    FluentAvalonia.UI.Controls.NavigationViewItemSeparator
{
    protected override Type StyleKeyOverride =>
        typeof(FluentAvalonia.UI.Controls.NavigationViewItemSeparator);
}