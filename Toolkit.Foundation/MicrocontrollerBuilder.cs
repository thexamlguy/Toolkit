using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;

namespace Toolkit.Foundation;

public class MicrocontrollerBuilder :
    IMicrocontrollerBuilder
{
    private readonly List<IMicrocontrollerBuilderConfiguration> configurations = [];

    public IReadOnlyCollection<IMicrocontrollerBuilderConfiguration> Configurations => 
        new ReadOnlyCollection<IMicrocontrollerBuilderConfiguration>(configurations);

    public IMicrocontrollerBuilderConfiguration<TConfiguration, TSerialReader, TRead, TReadDeserializer> Add<TConfiguration, TSerialReader, TRead, TReadDeserializer>(IConfiguration configuration)
        where TConfiguration : IMicrocontrollerConfiguration, new()
        where TSerialReader : SerialReader<TRead>
        where TReadDeserializer : IMicrocontrollerModuleDeserializer<TRead>, new()
    {
        MicrocontrollerBuilderConfiguration<TConfiguration, TSerialReader, TRead, TReadDeserializer>? builderConfiguration = new(configuration);
        configurations.Add(builderConfiguration);

        return builderConfiguration;
    }
}