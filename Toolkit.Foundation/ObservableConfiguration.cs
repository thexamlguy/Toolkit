using CommunityToolkit.Mvvm.Messaging;

namespace Toolkit.Foundation;

public partial class ObservableConfiguration<TConfiguration, TValue>(IServiceProvider provider,
    IServiceFactory factory,
    IMessenger messenger,
    IDisposer disposer,
    TConfiguration configuration,
    IWritableConfiguration<TConfiguration> writer,
    Func<TConfiguration, TValue?> read,
    Action<TValue?, TConfiguration> write) :
    Observable<TValue>(provider, factory, messenger, disposer),
    IHandler<ChangedEventArgs<TConfiguration>>
    where TConfiguration : class
{
    public void Handle(ChangedEventArgs<TConfiguration> args)
    {
        if (args.Sender is TConfiguration configuration)
        {
            Value = read(configuration);
        }
    }

    protected override void Activated() => Value = read(configuration);

    protected override void Changed(TValue? value)
    {
        if (IsActive)
        {
            writer.Write(args => write(value, args));
        }
    }
}