using CommunityToolkit.Mvvm.ComponentModel;

namespace Toolkit.Foundation;

public partial class ComponentConfigurationViewModel<TConfiguration, TValue, THeader, TDescription, TAction> :
    ValueViewModel<TValue>,
    IComponentConfigurationViewModel,
    INotificationHandler<ChangedEventArgs<TConfiguration>>
    where TConfiguration : class
{
    public ComponentConfigurationViewModel(IServiceProvider provider,
        IServiceFactory factory,
        IMediator mediator,
        IPublisher publisher,
        ISubscription subscriber,
        IDisposer disposer,
        THeader header,
        TDescription description,
        TAction action) : base(provider, factory, mediator, publisher, subscriber, disposer)
    {
    }

    public Task Handle(ChangedEventArgs<TConfiguration> args)
    {
        throw new NotImplementedException();
    }
}

public partial class ComponentConfigurationViewModel<TConfiguration, TValue, TAction>(IServiceProvider provider,
    IServiceFactory factory,
    IMediator mediator,
    IPublisher publisher,
    ISubscription subscriber,
    IDisposer disposer,
    TAction action,
    TConfiguration configuration,
    Func<TConfiguration, TValue> valueDelegate,
    object header,
    object description) :
    ValueViewModel<TValue>(provider, factory, mediator, publisher, subscriber, disposer),
    IComponentConfigurationViewModel,
    INotificationHandler<ChangedEventArgs<TConfiguration>>
    where TConfiguration : class
{
    [ObservableProperty]
    private TAction action = action;

    [ObservableProperty]
    private object header = header;

    [ObservableProperty]
    private object description = description;

    public override Task OnActivated()
    {
        Value = valueDelegate.Invoke(configuration);
        return base.OnActivated();
    }

    public Task Handle(ChangedEventArgs<TConfiguration> args)
    {
        if (args.Value is TConfiguration configuration)
        {
            Value = valueDelegate.Invoke(configuration);
        }

        return Task.CompletedTask;
    }
}

public partial class ComponentConfigurationViewModel<TConfiguration, TValue, TDescription, TAction>(IServiceProvider provider,
    IServiceFactory factory,
    IMediator mediator,
    IPublisher publisher,
    ISubscription subscriber,
    IDisposer disposer,
    TAction action,
    TDescription description,
    TConfiguration configuration,
    Func<TConfiguration, TValue> valueDelegate,
    object header) :
    ValueViewModel<TValue>(provider, factory, mediator, publisher, subscriber, disposer),
    IComponentConfigurationViewModel,
    INotificationHandler<ChangedEventArgs<TConfiguration>>
    where TConfiguration : class
{
    [ObservableProperty]
    private TAction action = action;

    [ObservableProperty]
    private object header = header;

    [ObservableProperty]
    private TDescription description = description;

    public override Task OnActivated()
    {
        Value = valueDelegate.Invoke(configuration);
        return base.OnActivated();
    }

    public Task Handle(ChangedEventArgs<TConfiguration> args)
    {
        if (args.Value is TConfiguration configuration)
        {
            Value = valueDelegate.Invoke(configuration);
        }

        return Task.CompletedTask;
    }
}