using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using Toolkit.Foundation;
using Windows.UI.Xaml.Markup;

namespace Toolkit.UI.WinUI;

[ContentProperty(Name = nameof(Actions))]
public class NavigateRegionAction :
    DependencyObject,
    IAction
{
    public static readonly DependencyProperty ActionsProperty =
        DependencyProperty.Register(nameof(Actions),
            typeof(ActionCollection), typeof(NavigateRegionAction),
                new PropertyMetadata(null));

    public static readonly DependencyProperty NameProperty =
        DependencyProperty.Register(nameof(Name),
            typeof(string), typeof(NavigateRegionAction),
                new PropertyMetadata(null));

    private ActionCollection? actions;

    public ActionCollection Actions => actions ??= [];

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public object? Execute(object? sender,
        object? parameter)
    {
        if (sender is Control control)
        {
            if (control.DataContext is IObservableViewModel observableViewModel)
            {
                if (observableViewModel.Provider.GetRequiredService<INavigationRegion>() 
                    is INavigationRegion navigationRegion)
                {
                    navigationRegion.Register(Name, sender);
                    Interaction.ExecuteActions(sender, Actions, parameter);
                }
            }
        }

        return true;
    }
}
