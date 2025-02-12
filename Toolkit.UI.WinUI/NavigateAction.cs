using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Toolkit.Foundation;
using Toolkit.WinUI;
using Windows.UI.Xaml.Markup;

namespace Toolkit.UI.WinUI;

[ContentProperty(Name = nameof(Parameters))]
public class NavigateAction :
    DependencyObject,
    IAction
{
    public static readonly DependencyProperty RegionProperty =
        DependencyProperty.Register(nameof(Region),
            typeof(object), typeof(NavigateAction),
                new PropertyMetadata(null));

    public static readonly DependencyProperty RouteProperty =
        DependencyProperty.Register(nameof(Route),
            typeof(string), typeof(NavigateAction),
                new PropertyMetadata(null));

    public static readonly DependencyProperty ScopeProperty =
        DependencyProperty.Register(nameof(Scope),
            typeof(NavigateScope), typeof(NavigateAction),
                new PropertyMetadata(NavigateScope.Default));

    public static readonly DependencyProperty ParametersProperty =
        DependencyProperty.Register(nameof(Parameters),
            typeof(ParameterCollection), typeof(NavigateAction),
                new PropertyMetadata(null));

    private ParameterCollection parameterCollection = [];

    public object Region
    {
        get => GetValue(RegionProperty);
        set => SetValue(RegionProperty, value);
    }

    public ParameterCollection Parameters =>
        parameterCollection ??= [];

    public string Route
    {
        get => (string)GetValue(RouteProperty);
        set => SetValue(RouteProperty, value);
    }

    public NavigateScope Scope
    {
        get => (NavigateScope)GetValue(ScopeProperty);
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

                observableViewModel.Messenger.Send(new NavigateEventArgs(Route, Region.Equals(this) ? content : Region, Scope,
                    content.DataContext, parameters));
            }
        }

        return true;
    }
}