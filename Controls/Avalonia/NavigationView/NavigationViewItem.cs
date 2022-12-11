using Avalonia.Styling;

namespace Toolkit.Controls.Avalonia;

public class NavigationViewItem : FluentAvalonia.UI.Controls.NavigationViewItem, IStyleable
{
    Type IStyleable.StyleKey => typeof(FluentAvalonia.UI.Controls.NavigationViewItem);
}
