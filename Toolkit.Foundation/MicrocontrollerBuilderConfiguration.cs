using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Toolkit.Foundation;

public class MicroControllerBuilderConfiguration<TConfiguration, TReader, TRead, TEvent> : 
    IMicroControllerBuilderConfiguration<TConfiguration, TReader, TRead, TEvent>
    where TConfiguration : ISerialConfiguration
    where TReader : SerialReader<TRead> 
    where TEvent : ISerialEventArgs<TRead>
{
    private readonly List<IMicroControllerModuleDescriptor> modules = [];

    public Func<IServiceProvider, IMicroControllerContext?> Factory => (IServiceProvider provider) => provider.GetService<IMicroControllerContextFactory>()!
        .Create<TConfiguration, TReader, TRead, TEvent>(Modules);

    public IReadOnlyCollection<IMicroControllerModuleDescriptor> Modules => new ReadOnlyCollection<IMicroControllerModuleDescriptor>(modules);

    public IMicroControllerBuilderConfiguration<TConfiguration, TReader, TRead, TEvent> AddModule<TModule>()
        where TModule : IMicroControllerModule, new()
    {
        modules.Add(new MicroControllerModuleDescriptor<TModule>());
        return this;
    }
}