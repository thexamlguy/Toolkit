using System;
using Toolkit.Foundation;

namespace Toolkit.UI.WinUI;

public class ValidationMessageConverter :
    ValueConverter<IReadOnlyIndexDictionary<string, string>, string?>
{
    public string? Property { get; set; }

    protected override string? ConvertTo(IReadOnlyIndexDictionary<string, string> value,
        Type? targetType,
        object? parameter,
        string? language)
    {
        if (Property is { Length: > 0 } && value.TryGetValue(Property, out string? message))
        {
            return message;
        }

        return default;
    }
}
