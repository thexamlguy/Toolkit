using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactions.Core;
using Toolkit.Foundation;

namespace Toolkit.UI.WinUI;

public class ComparisonCondition :
    DependencyObject,
    ICondition
{
    public static readonly DependencyProperty LeftOperandProperty =
        DependencyProperty.Register(nameof(LeftOperand),
            typeof(object), typeof(ComparisonCondition),
            new PropertyMetadata(null));

    public static readonly DependencyProperty OperatorProperty =
        DependencyProperty.Register(nameof(Operator),
            typeof(ComparisonConditionType), typeof(ComparisonCondition),
            new PropertyMetadata(null));

    public static readonly DependencyProperty RightOperandProperty =
        DependencyProperty.Register(nameof(RightOperand),
            typeof(object), typeof(ComparisonCondition),
            new PropertyMetadata(null));

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
        get => (ComparisonConditionType)GetValue(OperatorProperty);
        set => SetValue(OperatorProperty, value);
    }

    public bool Evaluate() =>
        ComparisonLogic.Evaluate(LeftOperand, Operator, RightOperand);
}