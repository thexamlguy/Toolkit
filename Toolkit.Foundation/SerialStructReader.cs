using System.Buffers;
using System.IO.Pipelines;

namespace Toolkit.Foundation;

public class SerialStructReader(Stream stream) :
    SerialReader<SerialStructEventArgs>(stream)
{
    private readonly PipeReader reader = PipeReader.Create(stream);

    public override async IAsyncEnumerable<SerialStructEventArgs> ReadAsync()
    {
        while (true)
        {
            ReadResult? result = default;

            try
            {
                result = await reader.ReadAsync();
            }
            catch (IOException)
            {
                continue;
            }
            catch (OperationCanceledException)
            {
                yield break;
            }
            catch (Exception)
            {
                yield break;
            }

            if (result.HasValue)
            {
                ReadOnlySequence<byte> buffer = result.Value.Buffer;
                while (TryParse(ref buffer, out SerialStructEventArgs serialEvent))
                {
                    yield return serialEvent;
                }

                reader.AdvanceTo(buffer.Start, buffer.End);
            }

        }
    }

    private bool TryParse(ref ReadOnlySequence<byte> buffer, out SerialStructEventArgs serialEvent)
    {
        SequenceReader<byte> reader = new(buffer);
        serialEvent = default!;

        if (reader.Remaining < 3)
            return false;

        if (!reader.TryRead(out byte type))
            return false;

        if (!reader.TryReadLittleEndian(out short value))
            return false;

        serialEvent = new SerialStructEventArgs(type, value);
        buffer = buffer.Slice(reader.Position);
        return true;
    }
}
