
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
    public async Task Handle(ChangedEventArgs<TConfiguration> args)
    {
        if (args.Sender is TConfiguration configuration)
        {
           // await Task.Run(() => Value = read(configuration));
        }
    }

    public override async Task OnActivated()
    {
        await Task.Run(() => Value = read(configuration));
        await base.OnActivated();
    }

    protected override async void OnChanged(TValue? value)
    {
        if (IsActivated)
        {
            await Task.Run(() => writer.Write(args => write(value, args)));
        }

        base.OnChanged(value);
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
    private readonly Func<TConfiguration, TValue?> read;
    private readonly Action<TValue?, TConfiguration> write;
    private readonly IWritableConfiguration<TConfiguration> writer;
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

    public async Task Handle(ChangedEventArgs<TConfiguration> args)
    {
        if (args.Sender is TConfiguration configuration)
        {
           // await Task.Run(() => Value = read(configuration));
        }
    }

    public override async Task OnActivated()
    {
        await Task.Run(() => Value = read(configuration));
        await base.OnActivated();
    }

    protected override async void OnChanged(TValue? value)
    {
        if (IsActivated)
        {
            await Task.Run(() => writer.Write(args => write(value, args)));
        }

        base.OnChanged(value);
    }
}