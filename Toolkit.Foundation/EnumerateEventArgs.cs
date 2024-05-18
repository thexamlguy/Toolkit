namespace Toolkit.Foundation;

public record EnumerateEventArgs<TValue> : 
    IEnumerate
{
    public object? Key { get; init; }

    public static Enumerate<TValue, TOptions> With<TOptions>(TOptions options) where TOptions : class
    {
        return new Enumerate<TValue, TOptions>(options);
    }
}