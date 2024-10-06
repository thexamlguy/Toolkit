namespace Toolkit.UI.Controls.Avalonia;

public class TaskDialog : 
    FluentAvalonia.UI.Controls.TaskDialog
{
    protected override Type StyleKeyOverride =>
        typeof(FluentAvalonia.UI.Controls.TaskDialog);
}