using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public partial class ObservableConfigurationCollection<TConfiguration, TValue, TViewModel> :
    ObservableCollection<TValue, TViewModel>,
    IRecipient<ChangedEventArgs<TConfiguration>>
    where TConfiguration : class
    where TViewModel : notnull,
    IDisposable
{
    private readonly TConfiguration configuration;
    private readonly Func<TConfiguration, TValue?> read;
    private readonly Action<TValue?, TConfiguration> write;
    private readonly IWritableConfiguration<TConfiguration> writer;
    private readonly IDispatcher dispatcher;

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

        dispatcher = provider.GetRequiredService<IDispatcher>();
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

        dispatcher = provider.GetRequiredService<IDispatcher>();
        Value = value;
    }

    public void Receive(ChangedEventArgs<TConfiguration> args)
    {
        if (args.Value is TConfiguration configuration)
        {
            dispatcher.Invoke(() => Value = read(configuration));
        }
    }

    protected override void Activated() => dispatcher.Invoke(() => Value = read(configuration));

    protected override void Changed(TValue? value)
    {
        if (IsActive)
        {
            writer.Write(args => write(value, args));
        }

        base.Changed(value);
    }
}
