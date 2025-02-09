namespace Toolkit.Foundation;

public abstract class SerialReader<TRead>(Stream stream)
{
    public Stream Stream { get; } = stream;

    public abstract IAsyncEnumerable<TRead> ReadAsync();
}
