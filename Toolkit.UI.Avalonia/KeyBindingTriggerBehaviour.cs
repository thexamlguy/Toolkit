using Avalonia;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using System.Windows.Input;

namespace Toolkit.UI.Avalonia;

public class KeyBindingTriggerBehaviour :
    Trigger<InputElement>,
    ICommand
{
    public static readonly StyledProperty<KeyGesture> GestureProperty =
        AvaloniaProperty.Register<KeyBindingTriggerBehaviour, KeyGesture>(nameof(Gesture));

    public static readonly StyledProperty<bool> IsEnabledProperty =
        AvaloniaProperty.Register<KeyBindingTriggerBehaviour, bool>(nameof(IsEnabled), true);

    public KeyGesture Gesture
    {
        get => GetValue(GestureProperty);
        set => SetValue(GestureProperty, value);
    }

    public bool IsEnabled
    {
        get => GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    public event EventHandler? CanExecuteChanged;

    protected override void OnAttached()
    {
        if (Gesture is not null)
        {
            KeyBinding keyBinding = new()
            {
                Gesture = Gesture,
                Command = this
            };

            AssociatedObject?.KeyBindings.Add(keyBinding);
        }

        base.OnAttached();
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        if (IsEnabled)
        {
            Interaction.ExecuteActions(AssociatedObject, Actions, null);
        }
    }
}