namespace Toolkit.Foundation;

public record Aggerate
{
    public static AggerateEventArgs<TValue, TOptions> With<TValue, TOptions>(TOptions options) where TOptions : class
    {
        return new AggerateEventArgs<TValue, TOptions>(options);
    }

    public static AggerateEventArgs<TValue> With<TValue>()
    {
        return new AggerateEventArgs<TValue>();
    }
}

public record AggerateEventArgs<TValue, TOptions>(TOptions? Options = null) :
    IAggerate
    where TOptions : class
{
    public object? Key { get; init; }

    public AggerateMode Mode { get; init; }
}

public record AggerateEventArgs<TValue> :
    IAggerate
{
    public object? Key { get; init; }

    public AggerateMode Mode { get; init; }
}