using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Toolkit.Foundation;

public class MicrocontrollerBuilderConfiguration<TConfiguration, TSerialReader, TRead, TReadDeserializer>(IConfiguration configuration) : 
    IMicrocontrollerBuilderConfiguration<TConfiguration, TSerialReader, TRead, TReadDeserializer>
    where TConfiguration : IMicrocontrollerConfiguration, new()
    where TSerialReader : SerialReader<TRead> where TReadDeserializer : IMicrocontrollerModuleDeserializer<TRead>, new()
{
    private readonly List<IMicrocontrollerModuleDescriptor> modules = new();

    public Func<IServiceProvider, IMicrocontrollerContext> Factory => (IServiceProvider provider) => provider.GetService<IMicrocontrollerFactory>()!.Create<TSerialReader, TRead, TReadDeserializer>(configuration.Get<TConfiguration>(), Modules);

    public IReadOnlyCollection<IMicrocontrollerModuleDescriptor> Modules => new ReadOnlyCollection<IMicrocontrollerModuleDescriptor>(modules);

    public IMicrocontrollerBuilderConfiguration<TConfiguration, TSerialReader, TRead, TReadDeserializer> AddModule<TModule>()
        where TModule : IMicrocontrollerModule, new()
    {
        modules.Add(new MicrocontrollerModuleDescriptor<TModule>());
        return this;
    }
}