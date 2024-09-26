namespace Toolkit.UI.Controls.Avalonia;

public class AppWindow : FluentAvalonia.UI.Windowing.AppWindow
{
    protected override Type StyleKeyOverride =>
        typeof(FluentAvalonia.UI.Windowing.AppWindow);
}