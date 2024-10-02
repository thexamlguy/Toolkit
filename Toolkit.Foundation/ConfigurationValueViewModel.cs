
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
    Observable<TValue>(provider, factory, mediator, publisher, subscriber, disposer),
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

public partial class ConfigurationValueViewModel<TConfiguration, TValue, TItem> :
    ObservableCollection<TValue, TItem>,
    INotificationHandler<ChangedEventArgs<TConfiguration>>
    where TConfiguration : class
    where TItem : notnull,
    IDisposable
{
    private readonly TConfiguration configuration;
    private readonly IWritableConfiguration<TConfiguration> writer;
    private readonly Func<TConfiguration, TValue?> read;
    private readonly Action<TValue?, TConfiguration> write;

    public ConfigurationValueViewModel(IServiceProvider provider, 
        IServiceFactory factory, 
        IMediator mediator, 
        IPublisher publisher,
        ISubscriber subscriber,
        IDisposer disposer,
        TConfiguration configuration,
        IWritableConfiguration<TConfiguration> writer,
        Func<TConfiguration, TValue?> read,
        Action<TValue?, TConfiguration> write,
        TValue? value = default) : base(provider, factory, mediator, publisher, subscriber, disposer, value)
    {
        this.configuration = configuration;
        this.writer = writer;
        this.read = read;
        this.write = write;

        Value = value;
    }

    public ConfigurationValueViewModel(IServiceProvider provider, 
        IServiceFactory factory,
        IMediator mediator,
        IPublisher publisher, 
        ISubscriber subscriber, 
        IDisposer disposer, 
        IEnumerable<TItem> items,
        TConfiguration configuration,
        IWritableConfiguration<TConfiguration> writer,
        Func<TConfiguration, TValue?> read,
        Action<TValue?, TConfiguration> write,
        TValue? value = default) : base(provider, factory, mediator, publisher, subscriber, disposer, items, value)
    {
        this.configuration = configuration;
        this.writer = writer;
        this.read = read;
        this.write = write;

        Value = value;
    }

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