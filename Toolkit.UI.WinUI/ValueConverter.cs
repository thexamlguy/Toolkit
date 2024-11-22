using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Markup;
using System;

namespace Toolkit.UI.WinUI;

public abstract class ValueConverter<TSource, TTarget> :
    MarkupExtension,
    IValueConverter
{
    public object? Convert(object value,
        Type targetType,
        object parameter,
        string language) =>
        ConvertTo((TSource)value, targetType, parameter, language);

    public object? ConvertBack(object value,
        Type targetType,
        object parameter,
        string language) =>
        ConvertBackTo((TTarget)value, targetType, parameter, language);

    public TTarget? Convert(TSource value) =>
        ConvertTo(value, null, null, null);

    public TSource? ConvertBack(TTarget value) =>
        ConvertBackTo(value, null, null, null);

    protected virtual TTarget? ConvertTo(TSource value,
        Type? targetType,
        object? parameter,
        string? language) => default;

    protected virtual TSource? ConvertBackTo(TTarget value,
        Type? targetType,
        object? parameter,
        string? language) => default;

    protected override object ProvideValue() => this;
}
