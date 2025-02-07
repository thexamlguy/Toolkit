namespace Toolkit.Foundation;

public class MicrocontrollerFactory(IServiceFactory factory,
    ISerialFactory serialFactory) : 
    IMicrocontrollerFactory
{
    private readonly Dictionary<ISerialConfiguration, IMicrocontrollerContext> cache = new();

    public IMicrocontrollerContext<TRead, TReadDeserializer> Create<TSerialReader, TRead, TReadDeserializer>(IMicrocontrollerConfiguration configuration, IReadOnlyCollection<IMicrocontrollerModuleDescriptor> modules) where TSerialReader : SerialReader<TRead> where TReadDeserializer : IMicrocontrollerModuleDeserializer<TRead>, new()
    {
        if (cache.TryGetValue(configuration, out IMicrocontrollerContext? context))
        {
            return (IMicrocontrollerContext<TRead, TReadDeserializer>)context;
        }

        ISerialContext<TSerialReader, TRead> serialContext = serialFactory.Create<TSerialReader, TRead>(configuration);

        context = factory.Create<MicrocontrollerContext<TRead, TReadDeserializer>>(modules, serialContext);
        cache.Add(configuration, context);

        return (IMicrocontrollerContext<TRead, TReadDeserializer>)context;
    }
}
