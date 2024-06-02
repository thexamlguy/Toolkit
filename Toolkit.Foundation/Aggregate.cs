namespace Toolkit.Foundation;

public record Aggregate
{
    public static AggerateEventArgs<TValue, TOptions> As<TValue, TOptions>(TOptions options)
        where TOptions : class => new(options);

    public static AggerateEventArgs<TValue> As<TValue>() =>
        new();
}
