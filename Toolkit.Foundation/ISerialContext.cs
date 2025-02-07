namespace Toolkit.Foundation;

public interface ISerialContext<TSerialReader, TContent> :
    ISerialContext where TSerialReader : SerialReader<TContent>;

public interface ISerialContext
{
    void Open();
}
