using Microsoft.UI.Xaml;
using System;

namespace Toolkit.UI.WinUI;

public class BoolToVisibilityConverter :
    ValueConverter<bool, Visibility>
{
    public Visibility TrueValue { get; set; } = Visibility.Visible;

    public Visibility FalseValue { get; set; } = Visibility.Collapsed;

    protected override Visibility ConvertTo(bool value,
        Type? targetType,
        object? parameter,
        string? language)
    {
        _ = bool.TryParse(value.ToString(), out bool parsed);
        return parsed ? TrueValue : FalseValue;
    }
}
