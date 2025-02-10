using System.IO.Ports;

namespace Toolkit.Foundation;

public class SerialPortStream : 
    Stream
{
    private readonly SerialPort serialPort;

    public SerialPortStream(SerialPort serialPort)
    {
        this.serialPort = serialPort ?? throw new ArgumentNullException(nameof(serialPort));
        if (!this.serialPort.IsOpen) this.serialPort.Open();
    }

    public override bool CanRead => serialPort.IsOpen && serialPort.BaseStream.CanRead;

    public override bool CanSeek => false;

    public override bool CanWrite => serialPort.IsOpen && serialPort.BaseStream.CanWrite;

    public bool HasData => serialPort.BytesToRead > 0;

    public override long Length => throw new NotSupportedException();

    public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

    public override void Flush() => serialPort.BaseStream.Flush();

    public override int Read(byte[] buffer, int offset, int count) =>
        HasData ? serialPort.BaseStream.Read(buffer, offset, count) : 0;

    public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        await WaitForDataAsync(cancellationToken);
        return await serialPort.BaseStream.ReadAsync(buffer, cancellationToken);
    }

    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        await WaitForDataAsync(cancellationToken);
        return await serialPort.BaseStream.ReadAsync(buffer.AsMemory(offset, count), cancellationToken);
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    public override void SetLength(long value) => throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count) =>
        serialPort.BaseStream.Write(buffer, offset, count);

    public override async ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) =>
        await serialPort.BaseStream.WriteAsync(buffer, cancellationToken);

    public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
        await serialPort.BaseStream.WriteAsync(buffer.AsMemory(offset, count), cancellationToken);

    protected override void Dispose(bool disposing)
    {
        if (disposing) serialPort?.Dispose();
        base.Dispose(disposing);
    }

    private async Task WaitForDataAsync(CancellationToken cancellationToken)
    {
        while (!HasData)
        {
            await Task.Yield();
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
