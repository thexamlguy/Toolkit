namespace Toolkit.UI.Controls.Avalonia;

public class ProgressRing :
    FluentAvalonia.UI.Controls.ProgressRing
{
    protected override Type StyleKeyOverride =>
        typeof(FluentAvalonia.UI.Controls.ProgressRing);
}