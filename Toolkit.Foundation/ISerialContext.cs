namespace Toolkit.Foundation;

public interface ISerialContext<TReader, TRead, TEvent> :
    ISerialContext
    where TReader : SerialReader<TRead>
    where TEvent : SerialEventArgs<TRead>, new();


public interface ISerialContext
{
    bool Open();
}
