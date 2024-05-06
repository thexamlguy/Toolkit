namespace Toolkit.Foundation;

public record Enumerate<TValue> : 
    IEnumerate
{
    public object? Key { get; init; }

    public EnumerateMode Mode { get; init; }

    public static Enumerate<TValue, TOptions> With<TOptions>(TOptions options) where TOptions : class
    {
        return new Enumerate<TValue, TOptions>(options);
    }
}

public record Enumerate<TValue, TOptions>(TOptions? Options = null) : 
    IEnumerate
    where TOptions : class
{
    public object? Key { get; init; }

    public EnumerateMode Mode { get; init; }
}