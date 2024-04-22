using Avalonia;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using System.Windows.Input;

namespace Toolkit.UI.Avalonia;

public class KeyBindingBehavior : 
    Trigger<InputElement>, 
    ICommand
{
    public static readonly StyledProperty<KeyBinding> KeyBindingProperty =
        AvaloniaProperty.Register<KeyBindingBehavior, KeyBinding>(nameof(KeyBinding));

    public KeyBinding KeyBinding
    {
        get => GetValue(KeyBindingProperty);
        set => SetValue(KeyBindingProperty, value);
    }

    public event EventHandler? CanExecuteChanged;

    protected override void OnAttached()
    {
        if (KeyBinding != null)
        {
            KeyBinding.Command = this;
            AssociatedObject?.KeyBindings.Add(KeyBinding);
        }

        base.OnAttached();
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) => 
        Interaction.ExecuteActions(AssociatedObject, Actions, null);
}
