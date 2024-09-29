using Avalonia;
using Avalonia.Xaml.Interactivity;
using System.Reactive;
using System.Reflection;

namespace Toolkit.UI.Avalonia;

public class EventListenerBehaviour :
    Trigger
{
    public static readonly StyledProperty<string> EventNameProperty =
        AvaloniaProperty.Register<EventListenerBehaviour, string>(nameof(EventName));

    public static readonly StyledProperty<object> SourceProperty =
        AvaloniaProperty.Register<EventListenerBehaviour, object>(nameof(Source));

    private Delegate? eventHandler;
    private object? resolvedSource;

    static EventListenerBehaviour()
    {
        EventNameProperty.Changed.Subscribe(new AnonymousObserver<AvaloniaPropertyChangedEventArgs<string>>(EventNamePropertyChanged));
        SourceProperty.Changed.Subscribe(new AnonymousObserver<AvaloniaPropertyChangedEventArgs<object>>(SourcePropertyChanged));
    }

    public string EventName
    {
        get => GetValue(EventNameProperty);
        set => SetValue(EventNameProperty, value);
    }

    public object Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    private static void EventNamePropertyChanged(AvaloniaPropertyChangedEventArgs<string> args)
    {
        if (args.Sender is EventListenerBehaviour behaviour)
        {
            if (args.OldValue.GetValueOrDefault() is string oldValue)
            {
                behaviour.UnregisterEvent(oldValue);
            }

            if (args.NewValue.GetValueOrDefault() is string newValue)
            {
                behaviour.RegisterEvent(newValue);
            }
        }
    }

    private static void SourcePropertyChanged(AvaloniaPropertyChangedEventArgs<object> args)
    {
        if (args.Sender is EventListenerBehaviour behaviour)
        {
            behaviour.SetResolvedSource(args.GetNewValue<object>());
        }
    }


    private void OnEventRaised(object? sender, EventArgs args)
    {
        if (IsEnabled)
        {
            Interaction.ExecuteActions(AssociatedObject, Actions, null);
        }
    }

    private void RegisterEvent(string eventName)
    {
        if (eventName is { Length: 0 })
        {
            return;
        }

        if (resolvedSource is null)
        {
            return;
        }

        Type sourceType = resolvedSource.GetType();
        if (sourceType.GetEvent(EventName) is EventInfo eventInfo)
        {
            eventInfo.AddEventHandler(resolvedSource, new EventHandler(OnEventRaised));
        }
    }

    private void SetResolvedSource(object? newSource)
    {
        if (resolvedSource == newSource)
        {
            return;
        }

        if (resolvedSource is not null)
        {
            UnregisterEvent(EventName);
        }

        resolvedSource = newSource;

        if (resolvedSource is not null)
        {
            RegisterEvent(EventName);
        }
    }

    private void UnregisterEvent(string eventName)
    {
        if (eventHandler is null)
        {
            return;
        }

        if (eventName is { Length: 0 })
        {
            return;
        }

        if (resolvedSource is null)
        {
            return;
        }

        Type sourceType = resolvedSource.GetType();
        if (sourceType.GetEvent(EventName) is EventInfo eventInfo)
        {
            eventInfo.RemoveEventHandler(resolvedSource, new EventHandler(OnEventRaised));
        }
    }
}
