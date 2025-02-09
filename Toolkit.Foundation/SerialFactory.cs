using Microsoft.Extensions.DependencyInjection;
using System.IO.Ports;

namespace Toolkit.Foundation;

public class SerialFactory(IServiceProvider provider,
    IServiceFactory factory) : 
    ISerialFactory
{
    private readonly Dictionary<ISerialConfiguration, ISerialContext> cache = [];

    public ISerialContext<TReader, TRead>? Create<TConfiguration, TReader, TRead>()
        where TConfiguration : ISerialConfiguration
        where TReader : SerialReader<TRead>
    {
        if (provider.GetRequiredService<TConfiguration>() is TConfiguration configuration)
        {
            if (cache.TryGetValue(configuration, out ISerialContext? context))
            {
                return (ISerialContext<TReader, TRead>)context;
            }

            SerialPort serialPort = new(configuration.PortName, configuration.BaudRate)
            {
                DtrEnable = true
            };

            SerialConnection connection = new(serialPort);
            SerialStreamer streamer = new(serialPort);

            context = factory.Create<SerialContext<TReader, TRead>>(connection, streamer);
            cache.Add(configuration, context);

            return (ISerialContext<TReader, TRead>)context;
        }

        return default;
    }
}
