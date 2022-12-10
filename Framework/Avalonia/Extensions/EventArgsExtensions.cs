using Avalonia.Data.Converters;
using System.Globalization;

namespace Toolkit.Framework.Avalonia;

public static class EventArgsExtensions
{
    public static dynamic? GetEventArguments(this EventArgs args, string? path, IValueConverter? converter, object? converterParameter)
    {
        return !string.IsNullOrWhiteSpace(path) ? GetEventArgsPropertyPathValue(args, path) : converter is not null ? converter.Convert(args, typeof(object), converterParameter, CultureInfo.CurrentCulture) : (dynamic)args;
    }

    private static object GetEventArgsPropertyPathValue(object args, string path)
    {
        object? value = args;
        if (path is { })
        {
            value = PropertyPathHelper.GetValue(args, path);
        }

        return value;
    }
}