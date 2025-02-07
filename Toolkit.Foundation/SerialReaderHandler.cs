using System.Buffers;

namespace Toolkit.Foundation;

public abstract class SerialReaderHandler<TRead> :
    IAsyncHandler<TRead, ReadOnlySequence<byte>> where TRead : class
{
    public abstract Task<ReadOnlySequence<byte>> Handle(TRead args, CancellationToken cancellationToken = default);
}