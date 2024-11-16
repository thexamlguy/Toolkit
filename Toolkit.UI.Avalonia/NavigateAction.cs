using Avalonia;
using Avalonia.Controls;
using Avalonia.Metadata;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.Immutable;
using Toolkit.Foundation;

namespace Toolkit.UI.Avalonia;

public class NavigateAction :
    AvaloniaObject,
    IAction
{
    public static readonly DirectProperty<NavigateAction, ParameterCollection> ParametersProperty =
        AvaloniaProperty.RegisterDirect<NavigateAction, ParameterCollection>(nameof(Parameters),
            x => x.Parameters);

    public static readonly StyledProperty<object> RegionProperty =
        AvaloniaProperty.Register<NavigateAction, object>(nameof(Region));

    public static readonly StyledProperty<string> RouteProperty =
        AvaloniaProperty.Register<NavigateAction, string>(nameof(Route));

    public static readonly StyledProperty<string> ScopeProperty =
        AvaloniaProperty.Register<NavigateAction, string>(nameof(Scope));

    private ParameterCollection parameterCollection = [];

    public event EventHandler? Navigated;

    public object Region
    {
        get => GetValue(RegionProperty);
        set => SetValue(RegionProperty, value);
    }

    [Content]
    public ParameterCollection Parameters =>
        parameterCollection ??= [];

    public string Route
    {
        get => GetValue(RouteProperty);
        set => SetValue(RouteProperty, value);
    }

    public string Scope
    {
        get => GetValue(ScopeProperty);
        set => SetValue(ScopeProperty, value);
    }

    public object Execute(object? sender,
        object? parameter)
    {
        if (sender is Control content)
        {
            Dictionary<string, object> arguments =
                new(StringComparer.InvariantCultureIgnoreCase);

            if (content.DataContext is IObservableViewModel observableViewModel)
            {
                ImmutableDictionary<string, object>? parameters = Parameters is { Count: > 0 } ? Parameters.ToImmutableDictionary(x => x.Key, x => x.Value) :
                    ImmutableDictionary<string, object>.Empty;

                observableViewModel.Messenger.Send(new NavigateEventArgs(Route, Region == this ? content : Region, Scope ?? null,
                    content.DataContext, Navigated, parameters));
            }
        }

        return true;
    }
}