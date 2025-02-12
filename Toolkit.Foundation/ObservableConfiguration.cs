using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public partial class ObservableConfiguration<TConfiguration, TValue> :
    Observable<TValue>,
    IRecipient<ChangedEventArgs<TConfiguration>>
    where TConfiguration : class
{
    private readonly TConfiguration configuration;
    private readonly IWritableConfiguration<TConfiguration> writer;
    private readonly Func<TConfiguration, TValue?> read;
    private readonly Action<TValue?, TConfiguration> write;
    private readonly IDispatcher dispatcher;

    public ObservableConfiguration(IServiceProvider provider,
        IServiceFactory factory,
        IMessenger messenger,
        IDisposer disposer,
        TConfiguration configuration,
        IWritableConfiguration<TConfiguration> writer,
        Func<TConfiguration, TValue?> read,
        Action<TValue?, TConfiguration> write,
        TValue? value = default) : base(provider, factory, messenger, disposer)
    {
        this.configuration = configuration;
        this.writer = writer;
        this.read = read;
        this.write = write;

        dispatcher = provider.GetRequiredService<IDispatcher>();
    }

    public void Receive(ChangedEventArgs<TConfiguration> args)
    {
        if (args.Value is TConfiguration configuration)
        {
            dispatcher.Invoke(() => Value = read(configuration));
        }
    }

    protected override void Activated() => dispatcher.Invoke(() => Value = read(configuration));

    protected override void ValueChanged(TValue? value)
    {
        if (IsActive)
        {
            writer.Write(args => write(value, args));
        }

        base.ValueChanged(value);
    }
}