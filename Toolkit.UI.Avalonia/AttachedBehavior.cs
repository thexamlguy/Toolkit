using Avalonia.Xaml.Interactivity;

namespace Toolkit.UI.Avalonia;

public class AttachedBehavior : Trigger
{
    protected override void OnAttachedToVisualTree()
    {
        Interaction.ExecuteActions(AssociatedObject, Actions, null);
        base.OnAttachedToVisualTree();
    }
}