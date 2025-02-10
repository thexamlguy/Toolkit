namespace Toolkit.Foundation;

public record SerialEventArgs<TValue>
{
    public TValue? Value { get; init; }
}