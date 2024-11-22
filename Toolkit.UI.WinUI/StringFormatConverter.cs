using System;
using System.Globalization;

namespace Toolkit.UI.WinUI;

public class StringFormatConverter : 
    ValueConverter<object, object>
{
    public string? StringFormat { get; set; }

    protected override object? ConvertTo(object value, 
        Type? targetType,
        object? parameter, 
        string? language)
    {
        if (value is null)
        {
            return null!;
        }

        if (string.IsNullOrEmpty(StringFormat))
        {
            return value.ToString()!;
        }

        try
        {
            CultureInfo culture = string.IsNullOrWhiteSpace(language) ? CultureInfo.InvariantCulture : new CultureInfo(language);
            return string.Format(culture, StringFormat, value);
        }
        catch
        {
            return value;
        }
    }
}
