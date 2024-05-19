namespace Toolkit.Foundation;

public record Aggerate<TValue, TOptions>(TOptions? Options = null) : 
    IAggerate
    where TOptions : class
{
    public object? Key { get; init; }

    public AggerateMode Mode { get; init; }
}