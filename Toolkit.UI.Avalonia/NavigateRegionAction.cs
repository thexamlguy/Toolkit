using Avalonia;
using Avalonia.Controls;
using Avalonia.Metadata;
using Avalonia.Xaml.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using Toolkit.Foundation;

namespace Toolkit.UI.Avalonia;

public class NavigateRegionAction :
    AvaloniaObject,
    IAction
{
    public static readonly DirectProperty<NavigateRegionAction, ActionCollection> ActionsProperty =
        AvaloniaProperty.RegisterDirect<NavigateRegionAction, ActionCollection>(nameof(Actions), x => x.Actions);

    public static readonly StyledProperty<string> NameProperty =
        AvaloniaProperty.Register<NavigateRegionAction, string>(nameof(Name));

    private ActionCollection? actions;

    [Content]
    public ActionCollection Actions => actions ??= [];
    public string Name
    {
        get => GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public object? Execute(object? sender, 
        object? parameter)
    {
        if (sender is Control control)
        {
            if (control.DataContext is IObservableViewModel observableViewModel)
            {   
                if (observableViewModel.Provider.GetRequiredService<INavigationRegion>() is INavigationRegion navigationRegion)
                {
                    navigationRegion.Register(Name, sender);
                    Interaction.ExecuteActions(sender, Actions, parameter);
                }
            }
        }

        return true;
    }
}
