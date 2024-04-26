using Avalonia;
using Avalonia.Metadata;
using Avalonia.Xaml.Interactivity;

namespace Toolkit.UI.Avalonia;

public class ConditionAction :
    AvaloniaObject,
    IAction
{
    public static readonly DirectProperty<ConditionAction, ActionCollection> ActionsProperty =
        AvaloniaProperty.RegisterDirect<ConditionAction, ActionCollection>(nameof(Actions),
            x => x.Actions);

    public static readonly StyledProperty<ICondition> ConditionProperty =
        AvaloniaProperty.Register<ConditionAction, ICondition>(nameof(Condition));

    private ActionCollection? actions;

    [Content]
    public ActionCollection Actions => actions ??= [];

    public ICondition Condition
    {
        get => GetValue(ConditionProperty);
        set => SetValue(ConditionProperty, value);
    }

    public object? Execute(object? sender, object? parameter)
    {
        bool? result = Condition?.Evaluate();
        if (result is true)
        {
            Interaction.ExecuteActions(sender, Actions, parameter);
        }

        return true;
    }
}