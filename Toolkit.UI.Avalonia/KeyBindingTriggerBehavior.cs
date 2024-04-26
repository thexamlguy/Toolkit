using Avalonia;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using System.Windows.Input;

namespace Toolkit.UI.Avalonia;

public class KeyBindingTriggerBehavior :
    Trigger<InputElement>,
    ICommand
{
    public static readonly StyledProperty<KeyGesture> GestureProperty =
        AvaloniaProperty.Register<KeyBindingTriggerBehavior, KeyGesture>(nameof(Gesture));

    public KeyGesture Gesture
    {
        get => GetValue(GestureProperty);
        set => SetValue(GestureProperty, value);
    }

    public event EventHandler? CanExecuteChanged;

    protected override void OnAttached()
    {
        if (Gesture is not null)
        {
            KeyBinding keyBinding = new KeyBinding
            {
                Gesture = Gesture,
                Command = this
            };

            AssociatedObject?.KeyBindings.Add(keyBinding);
        }

        base.OnAttached();
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) =>
        Interaction.ExecuteActions(AssociatedObject, Actions, null);
}