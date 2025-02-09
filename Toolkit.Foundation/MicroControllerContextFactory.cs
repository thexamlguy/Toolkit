using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class MicroControllerContextFactory(IServiceProvider provider,
    IServiceFactory factory,
    ISerialFactory serialFactory) : 
    IMicroControllerContextFactory
{
    private readonly Dictionary<ISerialConfiguration, IMicroControllerContext> cache = [];

    public IMicroControllerContext<TRead, THandler>? Create<TConfiguration, TReader, TRead, THandler>(IReadOnlyCollection<IMicroControllerModuleDescriptor> modules)
        where TConfiguration : ISerialConfiguration
        where TReader : SerialReader<TRead>
        where THandler : ISerialEventArgs<TRead>
    {
        if (provider.GetRequiredService<TConfiguration>() is TConfiguration configuration)
        {
            if (cache.TryGetValue(configuration, out IMicroControllerContext? context))
            {
                return (IMicroControllerContext<TRead, THandler>)context;
            }

            if (serialFactory.Create<TConfiguration, TReader, TRead>() is ISerialContext<TReader, TRead> serialContext)
            {
                context = factory.Create<MicroControllerContext<TRead, THandler>>(modules, serialContext);
                cache.Add(configuration, context);

                return (IMicroControllerContext<TRead, THandler>)context;
            }
        }

        return default;
    }
}
