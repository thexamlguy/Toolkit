using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using System.Globalization;

namespace Toolkit.UI.Avalonia;

public class BooleanToPasswordCharConverter :
    MarkupExtension,
    IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) =>
        this;

    public char PasswordChar { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is bool boolValue ? boolValue ? '\0' : PasswordChar : (object)PasswordChar;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
