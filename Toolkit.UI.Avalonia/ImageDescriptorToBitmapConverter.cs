using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using System.Globalization;
using Toolkit.Foundation;

namespace Toolkit.UI.Avalonia;

public class ImageDescriptorToBitmapConverter : 
    MarkupExtension, 
    IValueConverter
{
    public object? Convert(object? value, 
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        if (value is IImageDescriptor imageDescriptor)
        {
            return imageDescriptor.Image;
        }

        return default;
    }

    public object? ConvertBack(object? value,
        Type targetType, 
        object? parameter,
        CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
