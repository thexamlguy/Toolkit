namespace Toolkit.UI.Controls.Avalonia;

public class InfoBar :
    FluentAvalonia.UI.Controls.InfoBar
{
    protected override Type StyleKeyOverride =>
        typeof(FluentAvalonia.UI.Controls.InfoBar);
}