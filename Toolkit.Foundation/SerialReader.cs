namespace Toolkit.Foundation;

public abstract class SerialReader<TValue>(Stream stream)
{
    public Stream Stream { get; } = stream;

    public abstract IAsyncEnumerable<TValue> ReadAsync();
}
