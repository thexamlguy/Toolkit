using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using System.Globalization;

namespace Toolkit.UI.Avalonia;

public class NamedTypeConverter : 
    MarkupExtension, 
    IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var d = value is not null ? value.GetType().Name : (object?)null;
        return d;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
}
public class ConditionalExpression :
    AvaloniaObject,
    ICondition
{
    public static readonly StyledProperty<ConditionCollection> ConditionsProperty =
        AvaloniaProperty.Register<ConditionalExpression, ConditionCollection>(nameof(Conditions));

    public static readonly StyledProperty<ForwardChaining> ForwardChainingProperty =
        AvaloniaProperty.Register<ConditionalExpression, ForwardChaining>(nameof(ForwardChaining));

    public ConditionalExpression() =>
        SetValue(ConditionsProperty, []);

    [Content]
    public ConditionCollection Conditions =>
        GetValue(ConditionsProperty);

    public ForwardChaining ForwardChaining
    {
        get => GetValue(ForwardChainingProperty);
        set => SetValue(ForwardChainingProperty, value);
    }

    public bool Evaluate()
    {
        bool result = false;
        foreach (ComparisonCondition operation in this.Conditions)
        {
            result = operation.Evaluate();

            if (result == false && ForwardChaining == ForwardChaining.And)
            {
                return result;
            }

            if (result == true && ForwardChaining == ForwardChaining.Or)
            {
                return result;
            }
        }

        return result;
    }
}