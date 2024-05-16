using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Metadata;
using Avalonia.Xaml.Interactivity;
using Toolkit.Foundation;

namespace Toolkit.UI.Avalonia;

public class NavigateAction :
    AvaloniaObject,
    IAction
{
    public static readonly DirectProperty<NavigateAction, ParameterBindingCollection> ParameterBindingsProperty =
        AvaloniaProperty.RegisterDirect<NavigateAction, ParameterBindingCollection>(nameof(ParameterBindings),
            x => x.ParameterBindings);

    public static readonly StyledProperty<object[]?> ParametersProperty =
        AvaloniaProperty.Register<NavigateAction, object[]?>(nameof(Parameters));

    public static readonly StyledProperty<object> RegionProperty =
        AvaloniaProperty.Register<NavigateAction, object>(nameof(Region));

    public static readonly StyledProperty<string> RouteProperty =
        AvaloniaProperty.Register<NavigateAction, string>(nameof(Route));

    public static readonly StyledProperty<string> ScopeProperty =
        AvaloniaProperty.Register<NavigateAction, string>(nameof(Scope));

    private ParameterBindingCollection parameterCollection = [];

    public event EventHandler? Navigated;

    public object Region
    {
        get => GetValue(RegionProperty);
        set => SetValue(RegionProperty, value);
    }

    [Content]
    public ParameterBindingCollection ParameterBindings =>
        parameterCollection ??= [];

    public object[]? Parameters
    {
        get => GetValue(ParametersProperty);
        set => SetValue(ParametersProperty, value);
    }

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
                object[] parameters = [.. Parameters ?? Enumerable.Empty<object?>(),
                    ..
                    ParameterBindings is { Count: > 0 } ?
                    ParameterBindings.Select(binding => new KeyValuePair<string, object>(binding.Key, binding.Value)).ToArray() :
                    Enumerable.Empty<KeyValuePair<string, object>>()];

                observableViewModel.Publisher.Publish(new NavigateEventArgs(Route, Region == this ? content : Region, Scope ?? null,
                    content.DataContext, Navigated, parameters));
            }
        }

        return true;
    }
}