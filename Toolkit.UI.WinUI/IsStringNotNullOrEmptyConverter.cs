using System;

namespace Toolkit.UI.WinUI;

public class IsStringNotNullOrEmptyConverter : 
    ValueConverter<string, bool>
{
    protected override bool ConvertTo(string value, Type? targetType, object? parameter, string? language) => 
        !string.IsNullOrEmpty(value);
}