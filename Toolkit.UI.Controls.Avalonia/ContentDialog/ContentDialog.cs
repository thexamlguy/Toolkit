namespace Toolkit.UI.Controls.Avalonia;

public class ContentDialog :
    FluentAvalonia.UI.Controls.ContentDialog
{
    protected override Type StyleKeyOverride =>
        typeof(FluentAvalonia.UI.Controls.ContentDialog);
}