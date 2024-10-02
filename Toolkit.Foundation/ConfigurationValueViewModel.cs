namespace Toolkit.Foundation;

public partial class ConfigurationValueViewModel<TConfiguration, TValue>(IServiceProvider provider,
    IServiceFactory factory,
    IMediator mediator,
    IPublisher publisher,
    ISubscriber subscriber,
    IDisposer disposer,
    TConfiguration configuration,
    Func<TConfiguration, TValue> valueDelegate) :
    ValueViewModel<TValue>(provider, factory, mediator, publisher, subscriber, disposer),
    INotificationHandler<ChangedEventArgs<TConfiguration>>
    where TConfiguration : class
{
    private readonly TConfiguration configuration = configuration;

    private readonly Func<TConfiguration, TValue> valueDelegate = valueDelegate;

    public Task Handle(ChangedEventArgs<TConfiguration> args)
    {
        throw new NotImplementedException();
    }

    public override Task OnActivated()
    {
        Value = valueDelegate(configuration);
        return base.OnActivated();
    }
}