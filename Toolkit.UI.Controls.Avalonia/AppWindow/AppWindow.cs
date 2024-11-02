using Avalonia.Controls.Chrome;

namespace Toolkit.UI.Controls.Avalonia;

public class AppWindow : FluentAvalonia.UI.Windowing.AppWindow
{
    protected override Type StyleKeyOverride =>
        typeof(FluentAvalonia.UI.Windowing.AppWindow);

    public AppWindow()
    {
        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = FluentAvalonia.UI.Windowing.TitleBarHitTestType.Complex;
    }
}