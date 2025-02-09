namespace Toolkit.Foundation;

public interface ISerialFactory
{
    ISerialContext<TReader, TRead>? Create<TConfiguration, TReader, TRead>()
        where TConfiguration : ISerialConfiguration
        where TReader : SerialReader<TRead>;
}
