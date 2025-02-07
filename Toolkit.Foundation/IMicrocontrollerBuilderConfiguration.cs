namespace Toolkit.Foundation;

public interface IMicrocontrollerBuilderConfiguration
{
    IReadOnlyCollection<IMicrocontrollerModuleDescriptor> Modules { get; }

    Func<IServiceProvider, IMicrocontrollerContext> Factory { get; }
}

public interface IMicrocontrollerBuilderConfiguration<TConfiguration, TSerialReader, TRead, TReadDeserializer> :
    IMicrocontrollerBuilderConfiguration where TConfiguration : IMicrocontrollerConfiguration, new()
    where TSerialReader : SerialReader<TRead> where TReadDeserializer : IMicrocontrollerModuleDeserializer<TRead>, new()
{

    IMicrocontrollerBuilderConfiguration<TConfiguration, TSerialReader, TRead, TReadDeserializer> AddModule<TModule>()
        where TModule : IMicrocontrollerModule, new();
}
