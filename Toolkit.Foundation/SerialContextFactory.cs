using Microsoft.Extensions.DependencyInjection;
using System.IO.Ports;

namespace Toolkit.Foundation;

public class SerialContextFactory(IServiceProvider provider,
    IServiceFactory factory) : 
    ISerialContextFactory
{
    private readonly Dictionary<ISerialConfiguration, ISerialContext> cache = [];

    public ISerialContext<TReader, TValue, TEvent>? Create<TConfiguration, TReader, TValue, TEvent>()
        where TConfiguration : ISerialConfiguration
        where TReader : SerialReader<TValue>
        where TEvent : SerialEventArgs<TValue>, new()
    {
        if (provider.GetRequiredService<TConfiguration>() is TConfiguration configuration)
        {
            if (cache.TryGetValue(configuration, out ISerialContext? context))
            {
                return (ISerialContext<TReader, TValue, TEvent>)context;
            }

            SerialPort serialPort = new(configuration.PortName, configuration.BaudRate)
            {
                ReadTimeout = SerialPort.InfiniteTimeout,
                WriteTimeout = SerialPort.InfiniteTimeout,
                DtrEnable = false,
                RtsEnable = false
            };

            SerialConnection connection = new(serialPort);
            SerialStreamer streamer = new(serialPort);

            context = factory.Create<SerialContext<TReader, TValue, TEvent>>(connection, streamer);
            cache.Add(configuration, context);

            return (ISerialContext<TReader, TValue, TEvent>)context;
        }

        return default;
    }
}
