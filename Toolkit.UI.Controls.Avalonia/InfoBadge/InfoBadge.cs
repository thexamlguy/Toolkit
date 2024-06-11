namespace Toolkit.UI.Controls.Avalonia;

public class InfoBadge :
    FluentAvalonia.UI.Controls.InfoBadge
{
    protected override Type StyleKeyOverride =>
        typeof(FluentAvalonia.UI.Controls.InfoBadge);
}