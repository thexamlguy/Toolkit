using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using System.Globalization;

namespace Toolkit.UI.Avalonia;

public class FileSizeNameConverter : 
    MarkupExtension,
    IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) =>
        this;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is long fileSize)
        {
            string[] sizeSuffixes = ["bytes", "KB", "MB", "GB", "TB", "PB"];

            int order = 0;
            double adjustedSize = fileSize;

            while (adjustedSize >= 1024 && order < sizeSuffixes.Length - 1)
            {
                order++;
                adjustedSize /= 1024;
            }

            return $"{adjustedSize:0.##} {sizeSuffixes[order]}";
        }

        return "0 bytes";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => 
        throw new NotImplementedException();
}
