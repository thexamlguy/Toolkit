namespace Toolkit.Foundation;

public interface ISerialFactory
{
    ISerialContext<TSerialReader, TContent> Create<TSerialReader, TContent>(ISerialConfiguration configuration)
        where TSerialReader : SerialReader<TContent>;
}
