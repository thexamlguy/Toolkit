namespace Toolkit.Foundation;

public abstract class SerialReader<TContent>(Stream stream)
{
    public Stream Stream { get; } = stream;

    public abstract IAsyncEnumerable<TContent> ReadAsync();
}
