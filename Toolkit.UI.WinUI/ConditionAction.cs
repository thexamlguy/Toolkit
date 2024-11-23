using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using Microsoft.Xaml.Interactivity;
using Toolkit.Foundation;

namespace Toolkit.UI.WinUI;

[ContentProperty(Name = nameof(Actions))]
public class ConditionAction :
    DependencyObject,
    IAction
{
    public static readonly DependencyProperty ActionsProperty =
        DependencyProperty.Register(nameof(Actions),
            typeof(ActionCollection), typeof(ConditionAction),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ConditionProperty =
        DependencyProperty.Register(nameof(Condition),
            typeof(ICondition), typeof(ConditionAction),
            new PropertyMetadata(null));

    private ActionCollection? actions;

    public ActionCollection Actions => actions ??= [];

    public ICondition? Condition
    {
        get => (ICondition?)GetValue(ConditionProperty);
        set => SetValue(ConditionProperty, value);
    }

    public object? Execute(object? sender, object? parameter)
    {
        if (Condition?.Evaluate() == true)
        {
            Interaction.ExecuteActions(sender, Actions, parameter);
        }

        return true;
    }
}