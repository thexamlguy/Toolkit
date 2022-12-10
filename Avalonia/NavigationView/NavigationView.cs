using Avalonia.Styling;

namespace Toolkit.Controls.Avalonia;

public class NavigationView : FluentAvalonia.UI.Controls.NavigationView, IStyleable
{
    Type IStyleable.StyleKey => typeof(FluentAvalonia.UI.Controls.NavigationView);
}