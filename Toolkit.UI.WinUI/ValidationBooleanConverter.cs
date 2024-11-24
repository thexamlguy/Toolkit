using System;
using Toolkit.Foundation;

namespace Toolkit.UI.WinUI;

public class ValidationBooleanConverter :
    ValueConverter<IReadOnlyIndexDictionary<string, string>, bool>
{
    public string? Property { get; set; }

    public bool TrueValue { get; set; } = true;

    public bool FalseValue { get; set; } = false;

    protected override bool ConvertTo(IReadOnlyIndexDictionary<string, string> value,
        Type? targetType,
        object? parameter,
        string? language)
    {
        if (Property is { Length: > 0 } && value.ContainsKey(Property))
        {
            return TrueValue;
        }

        return FalseValue;
    }
}
