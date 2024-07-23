using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Toolkit.UI.Avalonia;

public class AttachedEventTriggerBehaviour : Trigger
{
    public static readonly StyledProperty<RoutedEvent> RoutedEventProperty =
        AvaloniaProperty.Register<AttachedEventTriggerBehaviour, RoutedEvent>(nameof(RoutedEvent));

    public RoutedEvent RoutedEvent
    {
        get => GetValue(RoutedEventProperty);
        set => SetValue(RoutedEventProperty, value);
    }

    protected override void OnAttached()
    {
        if (RoutedEvent is not null)
        {
            if (AssociatedObject is Interactive interactive)
            {
                interactive.AddHandler(RoutedEvent, Handle);
            }
        }

        base.OnAttached();
    }

    protected override void OnDetaching()
    {
        if (RoutedEvent is not null)
        {
            if (AssociatedObject is Interactive interactive)
            {
                interactive.RemoveHandler(RoutedEvent, Handle);
            }
        }

        base.OnDetaching();
    }

    private void Handle(object sender, RoutedEventArgs args) => 
        Interaction.ExecuteActions(AssociatedObject, Actions, null);
}