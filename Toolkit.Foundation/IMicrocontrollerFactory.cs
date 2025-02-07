namespace Toolkit.Foundation;

public interface IMicrocontrollerFactory
{
    IMicrocontrollerContext<TRead, TReadDeserializer> Create<TSerialReader, TRead, TReadDeserializer>(IMicrocontrollerConfiguration configuration, IReadOnlyCollection<IMicrocontrollerModuleDescriptor> modules)
        where TSerialReader : SerialReader<TRead> where TReadDeserializer : IMicrocontrollerModuleDeserializer<TRead>, new();
}
