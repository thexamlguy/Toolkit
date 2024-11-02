namespace Toolkit.UI.Controls.Avalonia;

public class NumberBox :
    FluentAvalonia.UI.Controls.NumberBox
{
    protected override Type StyleKeyOverride =>
        typeof(FluentAvalonia.UI.Controls.NumberBox);
}
