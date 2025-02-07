namespace Toolkit.Foundation;

public interface ISerialConnection
{
    bool IsOpen { get; }

    void Close();

    bool Open();
}
