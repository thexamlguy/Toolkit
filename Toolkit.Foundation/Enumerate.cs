namespace Toolkit.Foundation;

public record Enumerate<TValue, TOptions>(TOptions? Options = null) : 
    IEnumerate
    where TOptions : class
{
    public object? Key { get; init; }

    public EnumerateMode Mode { get; init; }
}