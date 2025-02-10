using System.Buffers;
using System.IO.Pipelines;
using System.Text;

namespace Toolkit.Foundation;

public class SerialLineReader(Stream stream) : 
    SerialReader<string>(stream)
{
    private readonly PipeReader reader = PipeReader.Create(stream);

    public override async IAsyncEnumerable<string> ReadAsync()
    {
        while (true)
        {
            ReadResult result;
            try
            {
                result = await reader.ReadAsync();
            }
            catch
            {
                continue;
            }

            ReadOnlySequence<byte> buffer = result.Buffer;
            while (TryReadLine(ref buffer, out ReadOnlySequence<byte> line))
            {
                yield return EncodingExtensions.GetString(Encoding.UTF8, line);
            }

            reader.AdvanceTo(buffer.Start, buffer.End);

            if (result.IsCompleted)
                break;
        }
    }

    private bool TryReadLine(ref ReadOnlySequence<byte> buffer,
        out ReadOnlySequence<byte> line)
    {
        SequencePosition? position = buffer.PositionOf((byte)'\n');
        if (position == null)
        {
            line = default;
            return false;
        }

        line = buffer.Slice(0, position.Value);
        buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
        return true;
    }
}
