using Avalonia;

namespace Toolkit.UI.Avalonia;

public class Parameter :
    AvaloniaObject
{
    public static readonly StyledProperty<string> KeyProperty =
        AvaloniaProperty.Register<Parameter, string>(nameof(Key));

    public static readonly StyledProperty<object> ValueProperty =
        AvaloniaProperty.Register<Parameter, object>(nameof(Value));

    public string Key
    {
        get => GetValue(KeyProperty);
        set => SetValue(KeyProperty, value);
    }

    public object Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
}