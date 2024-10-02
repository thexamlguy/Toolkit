namespace Toolkit.Foundation;

public partial class ConfigurationValueViewModel<TConfiguration, TValue>(IServiceProvider provider,
    IServiceFactory factory,
    IMediator mediator,
    IPublisher publisher,
    ISubscriber subscriber,
    IDisposer disposer,
    TConfiguration configuration,
    IWritableConfiguration<TConfiguration> writer,
    Func<TConfiguration, TValue?> read,
    Action<TValue?, TConfiguration> write) :
    ValueViewModel<TValue>(provider, factory, mediator, publisher, subscriber, disposer),
    INotificationHandler<ChangedEventArgs<TConfiguration>>
    where TConfiguration : class
{
    public Task Handle(ChangedEventArgs<TConfiguration> args)
    {

        throw new NotImplementedException();
    }

    protected override void OnChanged(TValue? value)
    {
        writer.Write(args => write(value, args));
        base.OnChanged(value);
    }

    public override Task OnActivated()
    {
        Value = read(configuration);
        return base.OnActivated();
    }
}