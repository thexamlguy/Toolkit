namespace Toolkit.Foundation;

public record Synchronize
{
    public static SynchronizeEventArgs<TValue, TOptions> As<TValue, TOptions>(TOptions options)
        where TOptions : class => new(options);

    public static SynchronizeEventArgs<TValue> As<TValue>() =>
        new();
}
