using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using Toolkit.Foundation;

namespace Toolkit.UI.WinUI;

[ContentProperty(Name = nameof(Conditions))]
public class ConditionalExpression :
    DependencyObject,
    ICondition
{
    public static readonly DependencyProperty ConditionsProperty =
        DependencyProperty.Register(nameof(Conditions),
            typeof(ConditionCollection), typeof(ConditionalExpression),
            new PropertyMetadata(new ConditionCollection()));

    public static readonly DependencyProperty ForwardChainingProperty =
        DependencyProperty.Register(nameof(ForwardChaining),
            typeof(ForwardChaining), typeof(ConditionalExpression),
            new PropertyMetadata(ForwardChaining.And));

    public ConditionalExpression()
    {
        SetValue(ConditionsProperty, new ConditionCollection());
    }

    public ConditionCollection Conditions =>
        (ConditionCollection)GetValue(ConditionsProperty);

    public ForwardChaining ForwardChaining
    {
        get => (ForwardChaining)GetValue(ForwardChainingProperty);
        set => SetValue(ForwardChainingProperty, value);
    }

    public bool Evaluate()
    {
        bool result = false;
        foreach (var operation in Conditions)
        {
            result = operation.Evaluate();

            if (!result && ForwardChaining == ForwardChaining.And)
            {
                return false;
            }

            if (result && ForwardChaining == ForwardChaining.Or)
            {
                return true;
            }
        }

        return result;
    }
}