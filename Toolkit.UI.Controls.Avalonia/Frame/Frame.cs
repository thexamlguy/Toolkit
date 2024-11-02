namespace Toolkit.UI.Controls.Avalonia;

public class Frame :
    FluentAvalonia.UI.Controls.Frame
{
    protected override Type StyleKeyOverride =>
        typeof(FluentAvalonia.UI.Controls.Frame);
}