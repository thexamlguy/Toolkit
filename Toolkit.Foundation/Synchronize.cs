namespace Toolkit.Foundation;

public record Synchronize
{
    public static SynchronizeEventArgs<TValue, TOptions> As<TValue, TOptions>(TOptions options) => new(options);

    public static SynchronizeEventArgs<TValue> As<TValue>() =>
        new();
}
