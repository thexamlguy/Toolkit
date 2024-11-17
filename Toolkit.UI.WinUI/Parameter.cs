using Microsoft.UI.Xaml;
using System.Xml.Linq;

namespace Toolkit.WinUI;

public class Parameter :
    DependencyObject
{
    public static readonly DependencyProperty KeyProperty =
        DependencyProperty.Register(nameof(Key),
            typeof(object), typeof(Parameter),
                new PropertyMetadata(null));

    public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value),
            typeof(string), typeof(Parameter),
                new PropertyMetadata(null));

    public string Key
    {
        get => (string)GetValue(KeyProperty);
        set => SetValue(KeyProperty, value);
    }

    public object Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
}