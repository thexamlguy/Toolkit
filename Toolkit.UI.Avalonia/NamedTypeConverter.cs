using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using System.Globalization;

namespace Toolkit.UI.Avalonia;

public class NamedTypeConverter :
    MarkupExtension,
    IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => 
        value is not null ? value.GetType().Name : (object?)null;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => 
        throw new NotImplementedException();

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}