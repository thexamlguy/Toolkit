using System;
using System.Globalization;

namespace Toolkit.UI.WinUI;

public class StringFormatConverter : 
    ValueConverter<object?, object?>
{
    public string? StringFormat { get; set; }

    protected override object? ConvertTo(object? value, 
        Type? targetType,
        object? parameter, 
        string? language)
    {
        if (value is null) return null!;
        if (StringFormat is not { Length: > 0}) return $"{value}"!;

        try
        {
            CultureInfo culture = string.IsNullOrWhiteSpace(language) ? CultureInfo.InvariantCulture : new CultureInfo(language);

            if (value is TimeSpan timeSpan)
            {
                return timeSpan.ToString(StringFormat, culture);
            }

            return string.Format(culture, StringFormat, value);
        }
        catch
        {
            return value;
        }
    }
}