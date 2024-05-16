using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Xaml.Interactivity;
using Toolkit.Foundation;

namespace Toolkit.UI.Avalonia;

public class NavigateBackAction :
    AvaloniaObject,
    IAction
{
    public static readonly StyledProperty<string> RegionProperty =
        AvaloniaProperty.Register<NavigateBackAction, string>(nameof(Region));

    public static readonly StyledProperty<string> ScopeProperty =
        AvaloniaProperty.Register<NavigateBackAction, string>(nameof(Scope));

    public string Region
    {
        get => GetValue(RegionProperty);
        set => SetValue(RegionProperty, value);
    }

    public string Scope
    {
        get => GetValue(ScopeProperty);
        set => SetValue(ScopeProperty, value);
    }

    public object Execute(object? sender,
        object? parameter)
    {
        if (sender is TemplatedControl control)
        {
            if (control.DataContext is IObservableViewModel observableViewModel)
            {
                observableViewModel.Publisher.Publish(new NavigateBackEventArgs(Region
                    ?? null, Scope ?? null));
            }
        }

        return true;
    }
}