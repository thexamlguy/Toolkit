using Microsoft.Xaml.Interactivity;

namespace Toolkit.UI.WinUI;

public class AttachedBehaviour :
    Trigger
{
    protected override void OnAttached()
    {
        Interaction.ExecuteActions(AssociatedObject, Actions, null);
        base.OnAttached();
    }
}