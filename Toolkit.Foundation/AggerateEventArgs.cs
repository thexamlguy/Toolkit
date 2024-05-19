namespace Toolkit.Foundation;

public record AggerateEventArgs<TValue> : 
    IAggerate
{
    public object? Key { get; init; }

    public static Aggerate<TValue, TOptions> With<TOptions>(TOptions options) where TOptions : class
    {
        return new Aggerate<TValue, TOptions>(options);
    }
}