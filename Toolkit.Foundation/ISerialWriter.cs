namespace Toolkit.Foundation;

public interface ISerialWriter
{
    void Write(byte[] buffer, int offset, int count);
}
