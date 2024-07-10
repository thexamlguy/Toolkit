using Avalonia;
using Avalonia.Metadata;

namespace Toolkit.UI.Avalonia;
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