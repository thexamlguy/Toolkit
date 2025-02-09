namespace Toolkit.Foundation;

public interface ISerialContext<TReader, TRead> :
    ISerialContext
    where TReader : SerialReader<TRead>;

public interface ISerialContext
{
    void Open();
}
