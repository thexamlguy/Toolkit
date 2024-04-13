namespace Toolkit.UI.Controls.Avalonia;

public class NavigationView :
    FluentAvalonia.UI.Controls.NavigationView
{
    protected override Type StyleKeyOverride => 
        typeof(FluentAvalonia.UI.Controls.NavigationView);
}
