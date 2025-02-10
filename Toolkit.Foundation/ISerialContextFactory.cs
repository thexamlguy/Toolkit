namespace Toolkit.Foundation;

public interface ISerialContextFactory
{
    ISerialContext<TReader, TRead, TEvent>? Create<TConfiguration, TReader, TRead, TEvent>()
        where TConfiguration : ISerialConfiguration
        where TReader : SerialReader<TRead>
        where TEvent : SerialEventArgs<TRead>, new();
}
