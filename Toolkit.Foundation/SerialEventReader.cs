using System.Buffers;
using System.IO.Pipelines;

namespace Toolkit.Foundation;

public class SerialEventReader(Stream stream) : 
    SerialReader<SerialEventArgs>(stream)
{
    private readonly PipeReader reader = PipeReader.Create(stream);

    public override async IAsyncEnumerable<SerialEventArgs> ReadAsync()
    {
        while (true)
        {
            ReadResult result;
            try
            {
                result = await reader.ReadAsync();
            }
            catch (Exception)
            {
                continue;
            }

            ReadOnlySequence<byte> buffer = result.Buffer;

            while (TryParseEvent(ref buffer, out SerialEventArgs serialEvent))
            {
                yield return serialEvent;
            }

            reader.AdvanceTo(buffer.Start, buffer.End);

            if (result.IsCompleted)
                break;
        }

        await reader.CompleteAsync();
    }

    private bool TryParseEvent(ref ReadOnlySequence<byte> buffer, 
        out SerialEventArgs serialEvent)
    {
        SequenceReader<byte> reader = new(buffer);
        serialEvent = default!;

        if (reader.Remaining < 3)
            return false;

        if (!reader.TryRead(out byte type))
            return false;

        if (!reader.TryReadLittleEndian(out short value))
            return false;

        serialEvent = new SerialEventArgs(type, value);

        buffer = buffer.Slice(reader.Position);
        return true;
    }
}
