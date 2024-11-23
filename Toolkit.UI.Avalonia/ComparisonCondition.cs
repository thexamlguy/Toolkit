using Avalonia;
using Avalonia.Xaml.Interactivity;
using Toolkit.Foundation;

namespace Toolkit.UI.Avalonia;

public class ComparisonCondition :
    AvaloniaObject,
    ICondition
{
    public static readonly StyledProperty<object> LeftOperandProperty =
        AvaloniaProperty.Register<ComparisonCondition, object>(nameof(LeftOperand));

    public static readonly StyledProperty<ComparisonConditionType> OperatorProperty =
        AvaloniaProperty.Register<ComparisonCondition, ComparisonConditionType>(nameof(Operator));

    public static readonly StyledProperty<object> RightOperandProperty =
        AvaloniaProperty.Register<ComparisonCondition, object>(nameof(RightOperand));

    public object LeftOperand
    {
        get => GetValue(LeftOperandProperty);
        set => SetValue(LeftOperandProperty, value);
    }

    public object RightOperand
    {
        get => GetValue(RightOperandProperty);
        set => SetValue(RightOperandProperty, value);
    }

    public ComparisonConditionType Operator
    {
        get => GetValue(OperatorProperty);
        set => SetValue(OperatorProperty, value);
    }

    public bool Evaluate() => ComparisonLogic.Evaluate(LeftOperand,
        Operator, RightOperand);
}