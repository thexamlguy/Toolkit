using CommunityToolkit.Mvvm.Messaging;

namespace Toolkit.Foundation;

public partial class ObservableConfigurationCollection<TConfiguration, TValue, TViewModel> :
    ObservableCollection<TValue, TViewModel>,
    IHandler<ChangedEventArgs<TConfiguration>>
    where TConfiguration : class
    where TViewModel : notnull,
    IDisposable
{
    private readonly TConfiguration configuration;
    private readonly Func<TConfiguration, TValue?> read;
    private readonly Action<TValue?, TConfiguration> write;
    private readonly IWritableConfiguration<TConfiguration> writer;

    public ObservableConfigurationCollection(IServiceProvider provider,
        IServiceFactory factory,
        IMessenger messenger,
        IDisposer disposer,
        TConfiguration configuration,
        IWritableConfiguration<TConfiguration> writer,
        Func<TConfiguration, TValue?> read,
        Action<TValue?, TConfiguration> write,
        TValue? value = default) : base(provider, factory, messenger, disposer, value)
    {
        this.configuration = configuration;
        this.writer = writer;
        this.read = read;
        this.write = write;

        Value = value;
    }

    public ObservableConfigurationCollection(IServiceProvider provider,
        IServiceFactory factory,
        IMessenger messenger,
        IDisposer disposer,
        IEnumerable<TViewModel> items,
        TConfiguration configuration,
        IWritableConfiguration<TConfiguration> writer,
        Func<TConfiguration, TValue?> read,
        Action<TValue?, TConfiguration> write,
        TValue? value = default) : base(provider, factory, messenger, disposer, items, value)
    {
        this.configuration = configuration;
        this.writer = writer;
        this.read = read;
        this.write = write;

        Value = value;
    }

    public void Handle(ChangedEventArgs<TConfiguration> args)
    {
        if (args.Sender is TConfiguration configuration)
        {
            Value = read(configuration);
        }
    }

    protected override void Activated()
    {
        Value = read(configuration);
    }

    protected override void OnChanged(TValue? value)
    {
        if (IsActive)
        {
            writer.Write(args => write(value, args));
        }

        base.OnChanged(value);
    }
}
